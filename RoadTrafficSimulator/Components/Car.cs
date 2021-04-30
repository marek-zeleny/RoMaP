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

        private static int nextId = 0;
        private static readonly Millimetres minDistanceBetweenCars = 1.Metres();
        private static readonly MetresPerSecondPerSecond deceleration = 5.MetresPerSecondPerSecond();
        private static readonly MetresPerSecondPerSecond acceleration = 4.MetresPerSecondPerSecond();
        private static readonly Milliseconds reactionTime = 1.Seconds();

        private static MillimetresPerSecond CalculateOptimalSpeed(Millimetres freeSpace, MillimetresPerSecond carInFrontSpeed)
        {
            // Calculate optimal speed using the following formula, the result is nonnegative
            // v = sqrt((a_d t_r)^2 + 2 a_d (s_d - d) + v_f^2) - a_d t_r
            // For more details and derivation of this formula see the programming documentation
            // Has to be calculated in integers instead of the type system because of nonstandard intermediate units
            // (mm^2 / s^2), also need to use 64-bit integers because of large intermediate results
            // All defined multiplications and divisions should be done within the type system for safety

            // CAREFUL! Needs to be converted to mm^2 / s^2
            long x = (long)deceleration * (long)freeSpace * 1000; // a_d (s_d - d)
            long v_f = carInFrontSpeed;
            long v;
            if (v_f == 0)
            {
                // If v_f = 0 (the car approaches a crossroad or the car in front doesn't move), we also set t_r = 0
                // v = sqrt(2 a_d (s_d - d))
                v = (long)Math.Sqrt(2 * x);
            }
            else
            {
                long v_d = deceleration * reactionTime; // a_d * t_r
                v = (long)Math.Sqrt(v_d * v_d + 2 * x + v_f * v_f) - v_d;
            }
            Debug.Assert(v >= 0);
            return new MillimetresPerSecond((int)v);
        }

        #endregion static

        private readonly Action<Statistics> finishDriveAction;
        private Navigation navigation;
        private Statistics statistics;
        private Millimetres distance;
        private bool newRoad;
        private Milliseconds remainingTimeAfterCrossing;

        public int Id { get; }
        public Millimetres Length { get; }
        public MillimetresPerSecond CurrentSpeed { get; private set; }
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
            if (newRoad)
                newRoad = false;
            else
            {
                Millimetres drivenDistance = Move(time);
                CurrentSpeed = drivenDistance / time;
                statistics.Update(drivenDistance, CurrentSpeed);
                TryFinishDrive();
            }
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

        private Millimetres Move(Milliseconds time)
        {
            if (CarInFront == null)
                return ApproachCrossroad(time);
            else
                return ApproachCar(time);
        }

        private Millimetres ApproachCar(Milliseconds time)
        {
            Millimetres freeSpace = CarInFront.DistanceRear - distance - minDistanceBetweenCars;
            MillimetresPerSecond speed = CalculateOptimalSpeed(freeSpace, CarInFront.CurrentSpeed);

            MillimetresPerSecond maxSpeed = CurrentSpeed + acceleration * time;
            if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                maxSpeed = navigation.CurrentRoad.MaxSpeed;
            if (speed > maxSpeed)
                speed = maxSpeed;

            Millimetres travelledDistance = speed * time;
            Debug.Assert(travelledDistance <= freeSpace);
            distance += travelledDistance;
            return travelledDistance;
        }

        private Millimetres ApproachCrossroad(Milliseconds time)
        {
            Millimetres freeSpace = navigation.CurrentRoad.Length - distance;
            Millimetres travelledDistance;
            MillimetresPerSecond maxSpeed = CurrentSpeed + acceleration * time;
            if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                maxSpeed = navigation.CurrentRoad.MaxSpeed;
            if (navigation.CurrentRoad.Destination.CanCross(navigation.CurrentRoad.Id, navigation.NextRoad.Id))
            {
                travelledDistance = maxSpeed * time;
                if (travelledDistance >= freeSpace)
                {
                    Milliseconds remainingTime = (travelledDistance - freeSpace) / maxSpeed;
                    travelledDistance = freeSpace + TryCrossToNextRoad(remainingTime);
                }
            }
            else
            {
                MillimetresPerSecond speed = CalculateOptimalSpeed(freeSpace, 0.MetresPerSecond());
                if (speed > maxSpeed)
                    speed = maxSpeed;
                travelledDistance = speed * time;
                Debug.Assert(travelledDistance <= freeSpace);
            }
            return travelledDistance;
        }

        private Millimetres TryCrossToNextRoad(Milliseconds remainingTime)
        {
            if (!navigation.NextRoad.TryGetOn(this, out Car newCarInFront))
                return 0.Metres();

            newRoad = true;
            navigation.CurrentRoad.GetOff(this);
            navigation.MoveToNextRoad();
            statistics.MovedToNextRoad(navigation.CurrentRoad.Id);
            distance = 0.Metres();
            CarInFront = newCarInFront;
            CarBehind = null;
            Millimetres travelledDistance = Move(remainingTime);
            return travelledDistance;
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
            private Item<List<Timestamp<MillimetresPerSecond>>> speedLog = new Item<List<Timestamp<MillimetresPerSecond>>>(
                DetailLevel.High, new List<Timestamp<MillimetresPerSecond>>());

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
            public IReadOnlyList<Timestamp<MillimetresPerSecond>> SpeedLog { get => speedLog.Get(); }


            public Statistics(IClock clock, int CarId, Milliseconds expectedDuration, int firstRoadId)
                : base(clock)
            {
                this.CarId = CarId;
                this.expectedDuration.Set(expectedDuration);
                startTime.Set(clock.Time);
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, firstRoadId));
            }

            public void Update(Millimetres addedDistance, MillimetresPerSecond speed)
            {
                distance.Set(Distance + addedDistance);
                speedLog.Get()?.Add(new Timestamp<MillimetresPerSecond>(clock.Time, speed));
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
