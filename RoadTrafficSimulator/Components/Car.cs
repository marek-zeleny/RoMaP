using DataStructures.Graphs;
using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components
{
    class Car
    {
        private Navigation navigation;
        private Meters distance;
        private Car carInFront;

        public Meters Length { get; }
        public MetersPerSecond Speed { get; private set; }
        public Meters DistanceRear => distance - Length;

        public Car(Meters length, IGraph<int, int> map, Crossroad start, Crossroad finish)
        {
            Length = length;
            navigation = new Navigation(map, start, finish);
        }


        public bool RemoveCarInFront(Road authentication)
        {
            if (authentication != navigation.CurrentRoad)
                return false;
            carInFront = null;
            return true;
        }

        public void Tick()
        {
            Tick(1.Seconds());
        }

        public void Tick(Seconds time)
        {
            Speed = Move(time * navigation.CurrentRoad.MaxSpeed) / time;
        }

        private Meters Move(Meters maxDistance)
        {
            if (carInFront == null)
                return ApproachCrossroad(maxDistance);
            else
                return KeepDistance(maxDistance);
        }

        private Meters KeepDistance(Meters maxDistance)
        {
            Meters spaceInFront = carInFront.DistanceRear - distance;
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
            {
                FinishDrive();
                return spaceInFront;
            }
            return spaceInFront + CrossToNextRoad(maxDistance - spaceInFront);
        }

        private Meters CrossToNextRoad(Meters remainingDistance)
        {
            if (!navigation.CurrentRoad.Destination.CrossingAllowed(navigation.CurrentRoad.Id, navigation.NextRoad.Id)
                || !navigation.NextRoad.GetOn(this, out Car newCarInFront))
                return 0.Meters();

            navigation.CurrentRoad.GetOff(this);
            navigation.MoveToNextRoad();
            distance = 0.Meters();
            carInFront = newCarInFront;
            return Move(remainingDistance);
        }

        private void FinishDrive()
        {
            // TOTO
        }

        private struct Navigation
        {
            private IEnumerator<Road> remainingPath;
            private bool nextRoadExists;
            
            public Seconds ExpectedDuration { get; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get => nextRoadExists ? remainingPath.Current : null; }

            public Navigation(IGraph<int, int> map, Crossroad start, Crossroad finish)
            {
                var e = map.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, start, finish, out Weight expectedDuration);
                remainingPath = (IEnumerator<Road>)e.GetEnumerator();
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
