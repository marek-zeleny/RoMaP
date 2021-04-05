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
        private static int nextId = 0;

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
            navigation = new Navigation(map, start, finish, clock, out Seconds expectedDuration);
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

        public void Tick(Seconds time)
        {
            // If the car already crossed from a different road during this tick, do nothing
            if (!newRoad)
            {
                Meters drivenDistance = Move(time * navigation.CurrentRoad.MaxSpeed);
                CurrentSpeed = drivenDistance / time;
                statistics.Update(drivenDistance, CurrentSpeed);
                TryFinishDrive();
            }
        }

        public void FinishCrossingRoads(Seconds time)
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
            bool canCross = navigation.CurrentRoad.Destination.TrafficLight.DirectionAllowed(navigation.CurrentRoad.Id,
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
            private Item<Seconds> startTime = new Item<Seconds>(DetailLevel.Low);
            private Item<Seconds> finishTime = new Item<Seconds>(DetailLevel.Low);
            private Item<Meters> distance = new Item<Meters>(DetailLevel.Low, 0.Meters());
            private Item<Seconds> expectedDuration = new Item<Seconds>(DetailLevel.Low);
            private Item<List<Timestamp<int>>> roadLog = new Item<List<Timestamp<int>>>(DetailLevel.Medium,
                new List<Timestamp<int>>());
            private Item<List<Timestamp<MetersPerSecond>>> speedLog = new Item<List<Timestamp<MetersPerSecond>>>(
                DetailLevel.High, new List<Timestamp<MetersPerSecond>>());

            public int CarId { get; }
            public Seconds StartTime { get => startTime; }
            public Seconds FinishTime { get => finishTime; }
            public Meters Distance { get => distance; }
            public Seconds ExpectedDuration { get => expectedDuration; }
            public Seconds Duration { get => FinishTime - StartTime; }
            /// <summary>
            /// Records each road and time the car got on that road.
            /// </summary>
            public IReadOnlyList<Timestamp<int>> RoadLog { get => roadLog.Get(); }
            /// <summary>
            /// Periodically records the car's speed.
            /// </summary>
            public IReadOnlyList<Timestamp<MetersPerSecond>> SpeedLog { get => speedLog.Get(); }


            public Statistics(IClock clock, int CarId, Seconds expectedDuration, int firstRoadId)
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
