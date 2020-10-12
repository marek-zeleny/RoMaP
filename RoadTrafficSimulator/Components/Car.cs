﻿using System;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;
using System.Linq;

namespace RoadTrafficSimulator.Components
{
    class Car
    {
        private readonly Action<Car> finishDriveAction;
        private Navigation navigation;
        private Meters distance;

        public Meters Length { get; }
        public MetersPerSecond CurrentSpeed { get; private set; }
        public Meters DistanceRear { get => distance - Length; }
        private Car CarInFront { get; set; }
        public Car CarBehind { get; private set; }
        public Meters TotalDistance { get; private set; }
        public Seconds StartTime { get; }
        public Seconds ExpectedDuration { get => navigation.ExpectedDuration; }

        public Car(Meters length, IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish, Seconds time, Action<Car> finishDriveAction)
        {
            Length = length;
            navigation = new Navigation(map, start, finish);
            StartTime = time;
            this.finishDriveAction = finishDriveAction;
        }

        public bool Initialize()
        {
            if (!navigation.CurrentRoad.GetOn(this, out Car newCarInFront))
                return false;
            CarInFront = newCarInFront;
            return true;
        }

        public void Tick(Seconds time)
        {
            Meters drivenDistance = Move(time * navigation.CurrentRoad.MaxSpeed);
            TotalDistance += drivenDistance;
            CurrentSpeed = drivenDistance / time;
            if (navigation.NextRoad == null && distance == navigation.CurrentRoad.Length)
            {
                navigation.CurrentRoad.GetOff(this);
                finishDriveAction(this);
            }
        }

        public bool SetCarBehind(Road authentication, Car car)
        {
            if (authentication != navigation.CurrentRoad)
                return false;
            CarBehind = car;
            return true;
        }

        public bool RemoveCarInFront(Road authentication)
        {
            if (authentication != navigation.CurrentRoad)
                return false;
            CarInFront = null;
            return true;
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
            if (!navigation.CurrentRoad.Destination.TrafficLight.DirectionAllowed(navigation.CurrentRoad.Id, navigation.NextRoad.Id)
                || !navigation.NextRoad.GetOn(this, out Car newCarInFront))
                return 0.Meters();

            navigation.CurrentRoad.GetOff(this);
            navigation.MoveToNextRoad();
            distance = 0.Meters();
            CarInFront = newCarInFront;
            CarBehind = null;
            return Move(remainingDistance);
        }

        private class Navigation
        {
            private IEnumerator<Road> remainingPath;
            private bool nextRoadExists;
            
            public Seconds ExpectedDuration { get; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get => nextRoadExists ? remainingPath.Current : null; }

            public Navigation(IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish)
            {
                var e = map.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, start, finish, out Weight expectedDuration);
                remainingPath = e.Select(edge => (Road)edge).GetEnumerator();
                ExpectedDuration = ((int)expectedDuration).Seconds();
                if (!remainingPath.MoveNext())
                    throw new ArgumentException(string.Format("There doesn't exist any path from given {0} to {1} in the given {2}", nameof(start), nameof(finish), nameof(map)));
                CurrentRoad = remainingPath.Current;
                nextRoadExists = remainingPath.MoveNext();
            }

            public void MoveToNextRoad()
            {
                if (!nextRoadExists)
                    throw new InvalidOperationException(string.Format("Cannot move to the next road when the {0} is already at the end of the path.", nameof(Navigation)));
                CurrentRoad = remainingPath.Current;
                nextRoadExists = remainingPath.MoveNext();
            }
        }
    }
}