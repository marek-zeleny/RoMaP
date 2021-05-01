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
        private struct Timestamp
        {
            public Milliseconds time;
            public Algorithms.Path<Coords, int> path;
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

        private Algorithms.Path<Coords, int> GetFastestPath(Coords from, Coords to)
        {
            UpdateCache(from, to);
            return pathsCache[from][to].path;
        }

        private void UpdateCache(Coords from, Coords to)
        {
            if (pathsCache.ContainsKey(from)
                && pathsCache[from].ContainsKey(to)
                && pathsCache[from][to].time >= clock.Time)
                return;
            var paths = map.FindShortestPaths(Algorithms.GraphType.NonnegativeWeights, from,
                edge => ((Road)edge).AverageDuration.Weight());
            foreach (var (destination, path) in paths)
                pathsCache[from][destination] = new Timestamp { time = clock.Time, path = path };
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
                Debug.Assert(path.Weight < Weight.positiveInfinity);
                RemainingDuration = new Milliseconds((int)path.Weight);

                // TODO: try remainingPath = ((IEnumerable<Road>)e).GetEnumerator();
                remainingPath = path.Edges.Select(edge => (Road)edge).GetEnumerator();
                Debug.Assert(remainingPath.MoveNext());
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
            private CentralNavigation central;
            private Coords destination;

            public IClock Clock => central.clock;
            public Milliseconds RemainingDuration { get; private set; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get; private set; }

            public ActiveNavigation(CentralNavigation central, Coords start, Coords finish)
            {
                this.central = central;
                destination = finish;
                var path = central.GetFastestPath(start, finish);
                Debug.Assert(path.Weight < Weight.positiveInfinity);
                RemainingDuration = new Milliseconds((int)path.Weight);

                var edges = path.Edges.GetEnumerator();
                Debug.Assert(edges.MoveNext());
                CurrentRoad = (Road)edges.Current;
                if (edges.MoveNext())
                    NextRoad = (Road)edges.Current;
                else
                    NextRoad = null;
            }

            public void MoveToNextRoad()
            {
                if (NextRoad == null)
                    throw new InvalidOperationException("Cannot move to the next road when the navigation is already" +
                        "at the end of the path.");
                CurrentRoad = NextRoad;
                var path = central.GetFastestPath(CurrentRoad.ToNode.Id, destination);
                Debug.Assert(path.Weight < Weight.positiveInfinity);
                RemainingDuration = new Milliseconds((int)path.Weight);

                var edges = path.Edges.GetEnumerator();
                Debug.Assert(edges.MoveNext());
                NextRoad = (Road)edges.Current;
            }
        }
    }
}
