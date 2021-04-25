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

        private static readonly Meters minDistanceBetweenCars = 1.Meters();
        private static readonly MetersPerSecondPerSecond deceleration = 5.MetersPerSecondPerSecond();
        private static readonly MetersPerSecondPerSecond acceleration = 4.MetersPerSecondPerSecond();
        private static readonly Milliseconds minTimeInterval = 1.Seconds();
        private static readonly Milliseconds reactionTime = 1.Seconds();
        private static int nextId = 0;

        private static Meters CalculateBrakingDistance(MetersPerSecond speed)
        {
            // n = floor(v / (a_d * t_m))
            // b(v) = n * t_m * (v - 1/2 * a_d * t_m * (n + 1))
            // For more information and derivation of this formula see the programming documentation
            MetersPerSecond decelerationStep = deceleration * minTimeInterval;
            int steps = (int)speed / (decelerationStep);
            MetersPerSecond subtractedSpeed = (decelerationStep * (steps + 1)) / 2;
            Meters distance = steps * minTimeInterval * (speed - subtractedSpeed);
            return distance;
        }

        private static Meters CalculateBrakingDistanceDiff(MetersPerSecond speed1, MetersPerSecond speed2)
        {
            // Uses the same formula as the function above, but calculates the difference efficiently
            // b(v1) - b(v2) = t_m * (n * v1 - m * v2 + 1/2 * a_d * t_m * (m * (m + 1) - n * (n + 1)))
            // For derivation see the programming documentation
            MetersPerSecond decelerationStep = deceleration * minTimeInterval;
            int steps1 = (int)speed1 / decelerationStep;
            int steps2 = (int)speed2 / decelerationStep;
            // If n == m, the formula can be much simplified
            if (steps1 == steps2)
                return minTimeInterval * steps1 * (speed1 - speed2);
            int stepsSquareDiff = steps2 * (steps2 + 1) - steps1 * (steps1 + 1);
            MetersPerSecond speedDiff = steps1 * speed1 - steps2 * speed2;
            Meters distance = minTimeInterval * (speedDiff + (decelerationStep * stepsSquareDiff) / 2);
            return distance;
        }

        #endregion static

        private readonly Action<Statistics> finishDriveAction;
        private Navigation navigation;
        private Statistics statistics;
        private Meters distance;
        private bool newRoad;
        private Meters remainingDistanceAfterCrossing;

        public int Id { get; }
        public Meters Length { get; }
        public MetersPerSecond CurrentSpeed { get; private set; }
        public Meters DistanceRear { get => distance - Length; }
        private Car CarInFront { get; set; }
        public Car CarBehind { get; private set; }

        public Car(Meters length, IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish, IClock clock,
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
                MetersPerSecond maxSpeed = CurrentSpeed + acceleration * time;
                if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                    maxSpeed = navigation.CurrentRoad.MaxSpeed;
                Meters drivenDistance = Move(maxSpeed * time);
                CurrentSpeed = drivenDistance / time;
                statistics.Update(drivenDistance, CurrentSpeed);
                TryFinishDrive();
            }
        }

        public void FinishCrossingRoads(Milliseconds time)
        {
            if (newRoad && remainingDistanceAfterCrossing > 0)
            {
                Meters drivenDistance = Move(remainingDistanceAfterCrossing);
                remainingDistanceAfterCrossing = 0.Meters();
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

        private Meters Move(Meters maxDistance)
        {
            if (CarInFront == null)
                return ApproachCrossroad(maxDistance);
            else
                return KeepDistance(maxDistance);
        }

        private Meters KeepDistance(Meters maxDistance)
        {
            // Calculate the optimal speed using the following formula
            // v = (s_d - d + b(v') - b(v)) / t_r
            // For more details and derivation of this formula see the programming documentation
            Meters distanceBetweenCars = CarInFront.DistanceRear - distance;
            Meters brakingDistanceDiff = CalculateBrakingDistanceDiff(CurrentSpeed, CarInFront.CurrentSpeed);
            Meters freeDistance = distanceBetweenCars - minDistanceBetweenCars + brakingDistanceDiff;
            MetersPerSecond optimalSpeed = freeDistance / minTimeInterval;
            // TODO: use the calculated optimal speed (some refactoring with crossing roads needed)
            Meters spaceInFront = CarInFront.DistanceRear - distance;
            if (maxDistance < spaceInFront)
            {
                distance += maxDistance;
                return maxDistance;
            }
            distance += spaceInFront;
            return spaceInFront;
        }

        private Meters ApproachCrossroad(Meters maxDistance)
        {
            Meters spaceInFront = navigation.CurrentRoad.Length - distance;
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

        private Meters CrossToNextRoad(Meters remainingDistance)
        {
            MetersPerSecond oldSpeed = navigation.CurrentRoad.MaxSpeed;
            bool canCross = navigation.CurrentRoad.Destination.CanCross(navigation.CurrentRoad.Id,
                navigation.NextRoad.Id);
            if (!canCross
                || !navigation.NextRoad.TryGetOn(this, out Car newCarInFront))
                return 0.Meters();

            navigation.CurrentRoad.GetOff(this);
            navigation.MoveToNextRoad();
            statistics.MovedToNextRoad(navigation.CurrentRoad.Id);
            distance = 0.Meters();
            CarInFront = newCarInFront;
            CarBehind = null;
            MetersPerSecond newSpeed = navigation.CurrentRoad.MaxSpeed;
            remainingDistance *= (double)newSpeed / oldSpeed;
            Meters moved = Move(remainingDistance);
            remainingDistanceAfterCrossing = remainingDistance - moved;
            newRoad = true;
            return moved;
        }

        private void TryFinishDrive()
        {
            if (navigation.NextRoad == null && distance == navigation.CurrentRoad.Length)
            {
                CurrentSpeed = 0.MetersPerSecond();
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
            private Item<Meters> distance = new Item<Meters>(DetailLevel.Low, 0.Meters());
            private Item<Milliseconds> expectedDuration = new Item<Milliseconds>(DetailLevel.Low);
            private Item<List<Timestamp<int>>> roadLog = new Item<List<Timestamp<int>>>(DetailLevel.Medium,
                new List<Timestamp<int>>());
            private Item<List<Timestamp<MetersPerSecond>>> speedLog = new Item<List<Timestamp<MetersPerSecond>>>(
                DetailLevel.High, new List<Timestamp<MetersPerSecond>>());

            public int CarId { get; }
            public Milliseconds StartTime { get => startTime; }
            public Milliseconds FinishTime { get => finishTime; }
            public Meters Distance { get => distance; }
            public Milliseconds ExpectedDuration { get => expectedDuration; }
            public Milliseconds Duration { get => FinishTime - StartTime; }
            /// <summary>
            /// Records each road and time the car got on that road.
            /// </summary>
            public IReadOnlyList<Timestamp<int>> RoadLog { get => roadLog.Get(); }
            /// <summary>
            /// Periodically records the car's speed.
            /// </summary>
            public IReadOnlyList<Timestamp<MetersPerSecond>> SpeedLog { get => speedLog.Get(); }


            public Statistics(IClock clock, int CarId, Milliseconds expectedDuration, int firstRoadId)
                : base(clock)
            {
                this.CarId = CarId;
                this.expectedDuration.Set(expectedDuration);
                startTime.Set(clock.Time);
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, firstRoadId));
            }

            public void Update(Meters addedDistance, MetersPerSecond speed)
            {
                distance.Set(Distance + addedDistance);
                speedLog.Get()?.Add(new Timestamp<MetersPerSecond>(clock.Time, speed));
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
