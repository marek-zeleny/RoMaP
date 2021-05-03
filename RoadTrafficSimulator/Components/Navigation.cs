using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using DataStructures.Graphs;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Components
{
    interface INavigation
    {
        IClock Clock { get; }
        Milliseconds RemainingDuration { get; }
        Road CurrentRoad { get; }
        Road NextRoad { get; }
        void MoveToNextRoad();
    }

    class CentralNavigation
    {
        private readonly struct Timestamp
        {
            public readonly Milliseconds searchTime;
            public readonly Road nextRoad; // null if this is the destination node
            public readonly Weight remainingWeight;

            public Timestamp(Milliseconds searchTime, Road nextRoad, Weight remainingWeight)
            {
                this.searchTime = searchTime;
                this.nextRoad = nextRoad;
                this.remainingWeight = remainingWeight;
            }
        }

        private Map map;
        private IClock clock;
        private IDictionary<Coords, IDictionary<Coords, Timestamp>> pathsCache;

        public CentralNavigation(Map map, IClock clock)
        {
            this.map = map;
            this.clock = clock;
        }

        public INavigation GetNavigation(Coords from, Coords to, bool active)
        {
            if (active)
                return new ActiveNavigation(this, from, to);
            else
                return new PassiveNavigation(this, from, to);
        }

        private (Road nextRoad, Weight remainingWeight) GetNextRoad(Coords from, Coords destination)
        {
            UpdateCache(from, destination);
            Timestamp t = pathsCache[from][destination];
            return (t.nextRoad, t.remainingWeight);
        }

        private void UpdateCache(Coords from, Coords to)
        {
            if (pathsCache.ContainsKey(from)
                && pathsCache[from].ContainsKey(to)
                && pathsCache[from][to].searchTime >= clock.Time)
                return;

            var paths = map.FindShortestPaths(Algorithms.GraphType.NonnegativeWeights, from,
                edge => ((Road)edge).AverageDuration.Weight());
            foreach (var (dest, path) in paths)
            {
                Debug.Assert(path.TotalWeight < Weight.positiveInfinity);
                Road prevRoad = null;
                foreach (var segment in path.ReversedPathSegments)
                {
                    if (pathsCache[segment.Edge.ToNode.Id][dest].searchTime < clock.Time)
                        pathsCache[segment.Edge.ToNode.Id][dest] = new Timestamp(
                            clock.Time, prevRoad, path.TotalWeight - segment.TotalWeight);
                    prevRoad = (Road)segment.Edge;
                }
                pathsCache[from][dest] = new Timestamp(clock.Time, prevRoad, path.TotalWeight);
            }
        }

        private class PassiveNavigation : INavigation
        {
            private CentralNavigation central;
            private IEnumerator<Road> remainingPath;
            private bool nextRoadExists;

            public IClock Clock => central.clock;
            public Milliseconds RemainingDuration { get; private set; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get => nextRoadExists ? remainingPath.Current : null; }

            public PassiveNavigation(CentralNavigation central, Coords start, Coords finish)
            {
                this.central = central;
                var path = central.map.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, start, finish);
                Debug.Assert(path.TotalWeight < Weight.positiveInfinity);
                RemainingDuration = new Milliseconds((int)path.TotalWeight);

                // TODO: try remainingPath = ((IEnumerable<Road>)e).GetEnumerator();
                remainingPath = path.GetEdges().Select(edge => (Road)edge).GetEnumerator();
                bool success = remainingPath.MoveNext();
                Debug.Assert(success);
                CurrentRoad = remainingPath.Current;
                nextRoadExists = remainingPath.MoveNext();
            }

            public void MoveToNextRoad()
            {
                if (!nextRoadExists)
                    throw new InvalidOperationException("Cannot move to the next road when the navigation is already" +
                        "at the end of the path.");
                RemainingDuration -= new Milliseconds((int)CurrentRoad.Weight);
                CurrentRoad = remainingPath.Current;
                nextRoadExists = remainingPath.MoveNext();
            }
        }

        private class ActiveNavigation : INavigation
        {
            private readonly CentralNavigation central;
            private readonly Coords destination;
            private Milliseconds nextRemainingDuration;

            public IClock Clock => central.clock;
            public Milliseconds RemainingDuration { get; private set; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get; private set; }

            public ActiveNavigation(CentralNavigation central, Coords start, Coords finish)
            {
                this.central = central;
                destination = finish;

                var (firstRoad, totalWeight) = central.GetNextRoad(start, finish);
                RemainingDuration = new Milliseconds((int)totalWeight);
                CurrentRoad = firstRoad;
                UpdateNextRoad();
            }

            public void MoveToNextRoad()
            {
                if (NextRoad == null)
                    throw new InvalidOperationException("Cannot move to the next road when the navigation is already" +
                        "at the end of the path.");
                CurrentRoad = NextRoad;
                RemainingDuration = nextRemainingDuration;
                UpdateNextRoad();
            }

            private void UpdateNextRoad()
            {
                var (nextRoad, remainingWeight) = central.GetNextRoad(CurrentRoad.ToNode.Id, destination);
                nextRemainingDuration = new Milliseconds((int)remainingWeight);
                NextRoad = nextRoad;
            }
        }
    }
}
