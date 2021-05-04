using System;
using System.Collections.Generic;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    class Car
    {
        #region static

        private static int nextId = 0;
        private static readonly Distance minDistanceBetweenCars = 1.Metres();
        private static readonly Acceleration deceleration = 5.MetresPerSecondPerSecond();
        private static readonly Acceleration acceleration = 4.MetresPerSecondPerSecond();
        private static readonly Time reactionTime = 1.Seconds();

        private static Speed CalculateOptimalSpeed(Distance freeSpace, Speed carInFrontSpeed)
        {
            // Calculate optimal speed using the following formula, the result is non-negative
            // v = sqrt((a_d t_r)^2 + 2 a_d (s_d - d) + v_f^2) - a_d t_r
            // For more details and derivation of this formula see the programming documentation
            // Has to be calculated in integers instead of the type system because of non-standard intermediate units
            // (mm^2 / s^2), also need to use 64-bit integers because of large intermediate results
            // All defined multiplications and divisions should be done within the type system or use the precision
            // constants for correct conversion

            long convertCoef = (Speed.precision * Speed.precision) / (Acceleration.precision * Distance.precision);
            long x = (long)deceleration * (long)freeSpace * convertCoef; // a_d (s_d - d)
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
            return new Speed((int)v);
        }

        #endregion static

        private readonly Action<Statistics> finishDriveAction;
        private INavigation navigation;
        private Statistics statistics;
        private Distance distance;
        private bool newRoad;
        private Time remainingTimeAfterCrossing;

        public int Id { get; }
        public Distance Length { get; }
        public Speed CurrentSpeed { get; private set; }
        public Distance DistanceRear { get => distance - Length; }
        private Car CarInFront { get; set; }
        public Car CarBehind { get; private set; }

        public Car(Distance length, INavigation navigation, Action<Statistics> finishDriveAction)
        {
            Id = nextId++;
            Length = length;
            this.navigation = navigation;
            statistics = new Statistics(navigation.Clock, Id, navigation.RemainingDuration, navigation.CurrentRoad.Id);
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

        public void Tick(Time time)
        {
            // If the car already crossed from a different road during this tick, do nothing
            if (newRoad)
                newRoad = false;
            else
            {
                Distance drivenDistance = Move(time);
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

        private Distance Move(Time time)
        {
            if (CarInFront == null)
                return ApproachCrossroad(time);
            else
                return ApproachCar(time);
        }

        private Distance ApproachCar(Time time)
        {
            Distance freeSpace = CarInFront.DistanceRear - distance - minDistanceBetweenCars;
            Speed speed = CalculateOptimalSpeed(freeSpace, CarInFront.CurrentSpeed);

            Speed maxSpeed = CurrentSpeed + acceleration * time;
            if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                maxSpeed = navigation.CurrentRoad.MaxSpeed;
            if (speed > maxSpeed)
                speed = maxSpeed;

            Distance travelledDistance = speed * time;
            Debug.Assert(travelledDistance <= freeSpace);
            distance += travelledDistance;
            return travelledDistance;
        }

        private Distance ApproachCrossroad(Time time)
        {
            Distance freeSpace = navigation.CurrentRoad.Length - distance;
            Distance travelledDistance;
            Speed maxSpeed = CurrentSpeed + acceleration * time;
            if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                maxSpeed = navigation.CurrentRoad.MaxSpeed;
            if (navigation.CurrentRoad.Destination.CanCross(navigation.CurrentRoad.Id, navigation.NextRoad.Id))
            {
                travelledDistance = maxSpeed * time;
                if (travelledDistance >= freeSpace)
                {
                    Time remainingTime = (travelledDistance - freeSpace) / maxSpeed;
                    travelledDistance = freeSpace + TryCrossToNextRoad(remainingTime);
                }
            }
            else
            {
                Speed speed = CalculateOptimalSpeed(freeSpace, 0.MetresPerSecond());
                if (speed > maxSpeed)
                    speed = maxSpeed;
                travelledDistance = speed * time;
                Debug.Assert(travelledDistance <= freeSpace);
            }
            return travelledDistance;
        }

        private Distance TryCrossToNextRoad(Time remainingTime)
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
            Distance travelledDistance = Move(remainingTime);
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
            private Item<Time> startTime = new Item<Time>(DetailLevel.Low);
            private Item<Time> finishTime = new Item<Time>(DetailLevel.Low);
            private Item<Distance> distance = new Item<Distance>(DetailLevel.Low, 0.Metres());
            private Item<Time> expectedDuration = new Item<Time>(DetailLevel.Low);
            private Item<List<Timestamp<int>>> roadLog = new Item<List<Timestamp<int>>>(DetailLevel.Medium,
                new List<Timestamp<int>>());
            private Item<List<Timestamp<Speed>>> speedLog = new Item<List<Timestamp<Speed>>>(
                DetailLevel.High, new List<Timestamp<Speed>>());

            public int CarId { get; }
            public Time StartTime { get => startTime; }
            public Time FinishTime { get => finishTime; }
            public Distance Distance { get => distance; }
            public Time ExpectedDuration { get => expectedDuration; }
            public Time Duration { get => FinishTime - StartTime; }
            /// <summary>
            /// Records each road and time the car got on that road.
            /// </summary>
            public IReadOnlyList<Timestamp<int>> RoadLog { get => roadLog.Get(); }
            /// <summary>
            /// Periodically records the car's speed.
            /// </summary>
            public IReadOnlyList<Timestamp<Speed>> SpeedLog { get => speedLog.Get(); }


            public Statistics(IClock clock, int CarId, Time expectedDuration, int firstRoadId)
                : base(clock)
            {
                this.CarId = CarId;
                this.expectedDuration.Set(expectedDuration);
                startTime.Set(clock.Time);
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, firstRoadId));
            }

            public void Update(Distance addedDistance, Speed speed)
            {
                distance.Set(Distance + addedDistance);
                speedLog.Get()?.Add(new Timestamp<Speed>(clock.Time, speed));
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
