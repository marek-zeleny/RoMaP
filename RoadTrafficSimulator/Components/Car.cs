using System;
using System.Collections.Generic;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    class Car
    {
        #region static

        private static readonly Millimetres minDistanceBetweenCars = 1.Metres();
        private static readonly MetresPerSecondPerSecond deceleration = 5.MetresPerSecondPerSecond();
        private static readonly MetresPerSecondPerSecond acceleration = 4.MetresPerSecondPerSecond();
        // Can't be less than 1s, otherwise it will become 0 when multiplied by (any) acceleration
        private static readonly Milliseconds minTimeInterval = 1.Seconds();
        private static readonly Milliseconds reactionTime = 1.Seconds();
        private static int nextId = 0;

        private static Millimetres CalculateBrakingDistance(MetresPerSecond speed)
        {
            // n = floor(v / (a_d * t_m))
            // b(v) = n * t_m * (v - 1/2 * a_d * t_m * (n + 1))
            // For more information and derivation of this formula see the programming documentation
            MetresPerSecond decelerationStep = deceleration * minTimeInterval;
            int steps = (int)speed / (decelerationStep);
            MetresPerSecond subtractedSpeed = (decelerationStep * (steps + 1)) / 2;
            Millimetres distance = steps * minTimeInterval * (speed - subtractedSpeed);
            return distance;
        }

        private static Millimetres CalculateBrakingDistanceDiff(MetresPerSecond speed1, MetresPerSecond speed2)
        {
            // Uses the same formula as the function above, but calculates the difference efficiently
            // b(v1) - b(v2) = t_m * (n * v1 - m * v2 + 1/2 * a_d * t_m * (m * (m + 1) - n * (n + 1)))
            // For derivation see the programming documentation
            MetresPerSecond decelerationStep = deceleration * minTimeInterval;
            int steps1 = (int)speed1 / decelerationStep;
            int steps2 = (int)speed2 / decelerationStep;
            // If n == m, the formula can be much simplified
            if (steps1 == steps2)
                return minTimeInterval * steps1 * (speed1 - speed2);
            int stepsSquareDiff = steps2 * (steps2 + 1) - steps1 * (steps1 + 1);
            MetresPerSecond speedDiff = steps1 * speed1 - steps2 * speed2;
            Millimetres distance = minTimeInterval * (speedDiff + (decelerationStep * stepsSquareDiff) / 2);
            return distance;
        }

        #endregion static

        private readonly Action<Statistics> finishDriveAction;
        private Navigation navigation;
        private Statistics statistics;
        private Millimetres distance;
        private bool newRoad;
        private Millimetres remainingDistanceAfterCrossing;

        public int Id { get; }
        public Millimetres Length { get; }
        public MetresPerSecond CurrentSpeed { get; private set; }
        public Millimetres DistanceRear { get => distance - Length; }
        private Car CarInFront { get; set; }
        public Car CarBehind { get; private set; }

        public Car(Millimetres length, IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish, IClock clock,
            Action<Statistics> finishDriveAction)
        {
            Id = nextId++;
            Length = length;
            navigation = new Navigation(map, start, finish, clock, out Milliseconds expectedDuration);
            statistics = new Statistics(clock, Id, expectedDuration, navigation.CurrentRoad.Id);
            this.finishDriveAction = finishDriveAction;
        }

        #region methods

        public bool Initialise()
        {
            if (!navigation.CurrentRoad.TryGetOn(this, out Car newCarInFront))
                return false;
            CarInFront = newCarInFront;
            return true;
        }

        public void Tick(Milliseconds time)
        {
            // If the car already crossed from a different road during this tick, do nothing
            if (!newRoad)
            {
                MetresPerSecond maxSpeed = CurrentSpeed + acceleration * time;
                if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                    maxSpeed = navigation.CurrentRoad.MaxSpeed;
                Millimetres drivenDistance = Move(maxSpeed * time);
                CurrentSpeed = drivenDistance / time;
                statistics.Update(drivenDistance, CurrentSpeed);
                TryFinishDrive();
            }
        }

        public void FinishCrossingRoads(Milliseconds time)
        {
            if (newRoad && remainingDistanceAfterCrossing > 0)
            {
                Millimetres drivenDistance = Move(remainingDistanceAfterCrossing);
                remainingDistanceAfterCrossing = 0.Metres();
                CurrentSpeed += drivenDistance / time;
                TryFinishDrive();
            }
            newRoad = false;
        }

        public void SetCarBehind(Road road, Car car)
        {
            Debug.Assert(road == navigation.CurrentRoad);
            CarBehind = car;
        }

        public void RemoveCarInFront(Road road)
        {
            Debug.Assert(road == navigation.CurrentRoad);
            CarInFront = null;
        }

        private Millimetres Move(Millimetres maxDistance)
        {
            if (CarInFront == null)
                return ApproachCrossroad(maxDistance);
            else
                return KeepDistance(maxDistance);
        }

        private Millimetres KeepDistance(Millimetres maxDistance)
        {
            // Calculate the optimal speed using the following formula
            // v = (s_d - d + b(v') - b(v)) / t_r
            // For more details and derivation of this formula see the programming documentation
            Millimetres distanceBetweenCars = CarInFront.DistanceRear - distance;
            Millimetres brakingDistanceDiff = CalculateBrakingDistanceDiff(CurrentSpeed, CarInFront.CurrentSpeed);
            Millimetres freeDistance = distanceBetweenCars - minDistanceBetweenCars + brakingDistanceDiff;
            MetresPerSecond optimalSpeed = freeDistance / minTimeInterval;
            // TODO: use the calculated optimal speed (some refactoring with crossing roads needed)
            Millimetres spaceInFront = CarInFront.DistanceRear - distance;
            if (maxDistance < spaceInFront)
            {
                distance += maxDistance;
                return maxDistance;
            }
            distance += spaceInFront;
            return spaceInFront;
        }

        private Millimetres ApproachCrossroad(Millimetres maxDistance)
        {
            Millimetres spaceInFront = navigation.CurrentRoad.Length - distance;
            if (maxDistance <= spaceInFront)
            {
                distance += maxDistance;
                return maxDistance;
            }

            distance += spaceInFront;
            if (navigation.NextRoad == null)
                return spaceInFront;
            return spaceInFront + CrossToNextRoad(maxDistance - spaceInFront);
        }

        private Millimetres CrossToNextRoad(Millimetres remainingDistance)
        {
            MetresPerSecond oldSpeed = navigation.CurrentRoad.MaxSpeed;
            bool canCross = navigation.CurrentRoad.Destination.CanCross(navigation.CurrentRoad.Id,
                navigation.NextRoad.Id);
            if (!canCross
                || !navigation.NextRoad.TryGetOn(this, out Car newCarInFront))
                return 0.Metres();

            navigation.CurrentRoad.GetOff(this);
            navigation.MoveToNextRoad();
            statistics.MovedToNextRoad(navigation.CurrentRoad.Id);
            distance = 0.Metres();
            CarInFront = newCarInFront;
            CarBehind = null;
            MetresPerSecond newSpeed = navigation.CurrentRoad.MaxSpeed;
            remainingDistance *= (double)newSpeed / oldSpeed;
            Millimetres moved = Move(remainingDistance);
            remainingDistanceAfterCrossing = remainingDistance - moved;
            newRoad = true;
            return moved;
        }

        private void TryFinishDrive()
        {
            if (navigation.NextRoad == null && distance == navigation.CurrentRoad.Length)
            {
                CurrentSpeed = 0.MetresPerSecond();
                navigation.CurrentRoad.GetOff(this);
                statistics.Finish();
                finishDriveAction(statistics);
            }
        }

        #endregion methods

        public class Statistics : StatisticsBase
        {
            private Item<Milliseconds> startTime = new Item<Milliseconds>(DetailLevel.Low);
            private Item<Milliseconds> finishTime = new Item<Milliseconds>(DetailLevel.Low);
            private Item<Millimetres> distance = new Item<Millimetres>(DetailLevel.Low, 0.Metres());
            private Item<Milliseconds> expectedDuration = new Item<Milliseconds>(DetailLevel.Low);
            private Item<List<Timestamp<int>>> roadLog = new Item<List<Timestamp<int>>>(DetailLevel.Medium,
                new List<Timestamp<int>>());
            private Item<List<Timestamp<MetresPerSecond>>> speedLog = new Item<List<Timestamp<MetresPerSecond>>>(
                DetailLevel.High, new List<Timestamp<MetresPerSecond>>());

            public int CarId { get; }
            public Milliseconds StartTime { get => startTime; }
            public Milliseconds FinishTime { get => finishTime; }
            public Millimetres Distance { get => distance; }
            public Milliseconds ExpectedDuration { get => expectedDuration; }
            public Milliseconds Duration { get => FinishTime - StartTime; }
            /// <summary>
            /// Records each road and time the car got on that road.
            /// </summary>
            public IReadOnlyList<Timestamp<int>> RoadLog { get => roadLog.Get(); }
            /// <summary>
            /// Periodically records the car's speed.
            /// </summary>
            public IReadOnlyList<Timestamp<MetresPerSecond>> SpeedLog { get => speedLog.Get(); }


            public Statistics(IClock clock, int CarId, Milliseconds expectedDuration, int firstRoadId)
                : base(clock)
            {
                this.CarId = CarId;
                this.expectedDuration.Set(expectedDuration);
                startTime.Set(clock.Time);
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, firstRoadId));
            }

            public void Update(Millimetres addedDistance, MetresPerSecond speed)
            {
                distance.Set(Distance + addedDistance);
                speedLog.Get()?.Add(new Timestamp<MetresPerSecond>(clock.Time, speed));
            }

            public void MovedToNextRoad(int roadId)
            {
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, roadId));
            }

            public void Finish()
            {
                finishTime.Set(clock.Time);
            }
        }
    }
}
