using System;
using System.Collections.Generic;
using System.Linq;

using DataStructures.Graphs;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Components
{
    class Navigation
    {
        private IEnumerator<Road> remainingPath;
        private bool nextRoadExists;

        public Road CurrentRoad { get; private set; }
        public Road NextRoad { get => nextRoadExists ? remainingPath.Current : null; }

        public Navigation(IReadOnlyGraph<Coords, int> map, Crossroad start, Crossroad finish, IClock clock,
            out Milliseconds expectedDuration)
        {
            var e = map.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, start, finish,
                out Weight pathWeight);
            // TODO: try remainingPath = ((IEnumerable<Road>)e).GetEnumerator();
            remainingPath = e.Select(edge => (Road)edge).GetEnumerator();
            if (!remainingPath.MoveNext())
                throw new ArgumentException($"There doesn't exist any path from {nameof(start)} to" +
                    $"{nameof(finish)} in the {nameof(map)}.");
            CurrentRoad = remainingPath.Current;
            nextRoadExists = remainingPath.MoveNext();
            expectedDuration = new Milliseconds((int)pathWeight);
        }

        public void MoveToNextRoad()
        {
            if (!nextRoadExists)
                throw new InvalidOperationException("Cannot move to the next road when the navigation is already" +
                    "at the end of the path.");
            CurrentRoad = remainingPath.Current;
            nextRoadExists = remainingPath.MoveNext();
        }
    }
}
