using System;
using System.Collections.Generic;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;
using System.Linq;

namespace RoadTrafficSimulator.Components
{
    class Car
    {
        private readonly Action<Statistics> finishDriveAction;
        private Navigation navigation;
        private Meters distance;
        private bool newRoad;
        private Meters remainingDistanceAfterCrossing;

        public Meters Length { get; }
        public MetersPerSecond CurrentSpeed { get; private set; }
        public Meters DistanceRear { get => distance - Length; }
        private Car CarInFront { get; set; }
        public Car CarBehind { get; private set; }

        public Car(Meters length, IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish, IClock clock,
            Action<Statistics> finishDriveAction)
        {
            Length = length;
            navigation = new Navigation(map, start, finish, clock);
            this.finishDriveAction = finishDriveAction;
        }

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
                navigation.CurrentRoad.GetOff(this);
                navigation.Statistics.Finish();
                finishDriveAction(navigation.Statistics);
            }
        }

        private class Navigation
        {
            private IEnumerator<Road> remainingPath;
            private bool nextRoadExists;
            
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get => nextRoadExists ? remainingPath.Current : null; }
            public Statistics Statistics { get; }

            public Navigation(IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish, IClock clock)
            {
                var e = map.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, start, finish,
                    out Weight expectedDuration);
                remainingPath = e.Select(edge => (Road)edge).GetEnumerator();
                if (!remainingPath.MoveNext())
                    throw new ArgumentException($"There doesn't exist any path from {nameof(start)} to" +
                        $"{nameof(finish)} in the {nameof(map)}.");
                CurrentRoad = remainingPath.Current;
                nextRoadExists = remainingPath.MoveNext();
                Statistics = new Statistics(clock, ((int)expectedDuration).Seconds(), CurrentRoad);
            }

            public void MoveToNextRoad()
            {
                if (!nextRoadExists)
                    throw new InvalidOperationException("Cannot move to the next road when the navigation is already" +
                        "at the end of the path.");
                CurrentRoad = remainingPath.Current;
                Statistics.NextRoad(CurrentRoad);
                nextRoadExists = remainingPath.MoveNext();
            }
        }

        public class Statistics
        {
            public struct Timestamp
            {
                public Road Road { get; }
                public Seconds Time { get; }

                public Timestamp(Road road, Seconds time)
                {
                    Road = road;
                    Time = time;
                }

                public override string ToString() => $"{Road}; {Time}";
            }

            private IClock clock;

            /// <summary>
            /// Records each road and time the car got on that road.
            /// </summary>
            public List<Timestamp> RoadLog { get; private set; }
            public Meters Distance { get; private set; }
            public Seconds End { get; private set; }
            public Seconds ExpectedDuration { get; }
            public Seconds Duration { get => End - RoadLog[0].Time; }

            public Statistics(IClock clock, Seconds expectedDuration, Road firstRoad)
            {
                this.clock = clock;
                ExpectedDuration = expectedDuration;
                RoadLog = new List<Timestamp> { new Timestamp(firstRoad, clock.Time) };
            }

            public void NextRoad(Road road)
            {
                UpdateDistance();
                RoadLog.Add(new Timestamp(road, clock.Time));
            }

            public void Finish()
            {
                UpdateDistance();
                End = clock.Time;
            }

            private void UpdateDistance()
            {
                Distance += RoadLog[RoadLog.Count - 1].Road.Length;
            }
        }
    }
}
