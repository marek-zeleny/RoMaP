using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using DataStructures.Graphs;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents a navigation for a car.
    /// </summary>
    interface INavigation
    {
        /// <summary>
        /// Global clock measuring the running simulation's time
        /// </summary>
        IClock Clock { get; }
        /// <summary>
        /// Time estimation of the rest of the planned trip
        /// </summary>
        Time RemainingDuration { get; }
        /// <summary>
        /// Road on which the car is currently driving, or <c>null</c> if the drive is finished
        /// </summary>
        Road CurrentRoad { get; }
        /// <summary>
        /// Road where the car should continue when it reaches the end of the current road, or <c>null</c> if there
        /// is no next road
        /// </summary>
        Road NextRoad { get; }

        /// <summary>
        /// Sets the next road as the new current road and decides where to continue afterwards.
        /// </summary>
        void MoveToNextRoad();
    }

    /// <summary>
    /// Represents a central navigation system for optimised searching for shortest paths.
    /// It's used to create instances of navigations for cars.
    /// </summary>
    /// <remarks>
    /// This class serves active navigations to find shortest paths based on current traffic in the simulation.
    /// </remarks>
    class CentralNavigation
    {
        private readonly struct Timestamp
        {
            public readonly Time searchTime;
            public readonly Road nextRoad; // null if this is the destination node
            public readonly Weight remainingWeight;

            public Timestamp(Time searchTime, Road nextRoad, Weight remainingWeight)
            {
                this.searchTime = searchTime;
                this.nextRoad = nextRoad;
                this.remainingWeight = remainingWeight;
            }
        }

        private Map map;
        private IClock clock;
        private IDictionary<Coords, IDictionary<Coords, Timestamp>> pathsCache;

        /// <summary>
        /// Creates a new central navigation over a given map with a given simulation clock.
        /// </summary>
        public CentralNavigation(Map map, IClock clock)
        {
            this.map = map;
            this.clock = clock;
            pathsCache = new Dictionary<Coords, IDictionary<Coords, Timestamp>>(map.CrossroadCount);
            foreach (var crossroad in map.GetNodes())
                pathsCache[crossroad.Id] = new Dictionary<Coords, Timestamp>(map.CrossroadCount);
        }

        /// <summary>
        /// Obtains a new navigation guiding between two given coordinates denoting crossroads.
        /// </summary>
        /// <remarks>
        /// Passive navigation finds the shortest path with respect to maximal allowed speed on roads; as this speed
        /// doesn't change during the simulation, it only finds the path once at the beginning.
        /// Active navigation takes into account current traffic situation in the simulation and therefore updates the
        /// shortest path during the car's passage.
        /// </remarks>
        /// <param name="active">If <c>true</c>, returns an active navigation, otherwise returns a passive one</param>
        /// <exception cref="ArgumentException">
        /// There is no crossroad in the map under one of the given coordinates.
        /// </exception>
        public INavigation GetNavigation(Coords from, Coords to, bool active)
        {
            if (active)
                return new ActiveNavigation(this, from, to);
            else
                return new PassiveNavigation(this, from, to);
        }

        /// <summary>
        /// Gets the next road on the shortest path between given coordinates based on current traffic in the
        /// simulation.
        /// </summary>
        /// <returns>The next road and the weight of the shortest path found</returns>
        private (Road nextRoad, Weight remainingWeight) GetNextRoad(Coords from, Coords destination)
        {
            UpdateCache(from, destination);
            Timestamp t = pathsCache[from][destination];
            return (t.nextRoad, t.remainingWeight);
        }

        /// <summary>
        /// Updates cached shortest path between given coordinates. Does nothing if the cache is up-to-date.
        /// </summary>
        private void UpdateCache(Coords from, Coords to)
        {
            bool isUpToDate(Coords from, Coords to) =>
                pathsCache[from].ContainsKey(to) && pathsCache[from][to].searchTime >= clock.Time;

            if (isUpToDate(from, to))
                return;

            var paths = map.FindShortestPaths(Algorithms.GraphType.NonnegativeWeights, from,
                edge => ((Road)edge).AverageSpeed.Weight());
            foreach (var (dest, path) in paths)
            {
                Debug.Assert(path.TotalWeight < Weight.positiveInfinity);
                Road prevRoad = null;
                foreach (var segment in path.ReversedPathSegments)
                {
                    var source = segment.Edge.ToNode.Id;
                    if (!isUpToDate(source, dest))
                        pathsCache[source][dest] = new Timestamp(
                            clock.Time, prevRoad, path.TotalWeight - segment.TotalWeight);
                    prevRoad = (Road)segment.Edge;
                }
                if (!isUpToDate(from, dest))
                    pathsCache[from][dest] = new Timestamp(clock.Time, prevRoad, path.TotalWeight);
            }
        }

        #region nested_classes

        /// <summary>
        /// Represents a passive car navigation.
        /// </summary>
        /// <remarks>
        /// Finds the shortest path with respect to maximal allowed speed on roads; as this speed doesn't change during
        /// the simulation, it only finds the path once at the beginning and stores it for the rest of the passage.
        /// </remarks>
        private class PassiveNavigation : INavigation
        {
            private CentralNavigation central;
            private IEnumerator<Road> remainingPath;
            private bool nextRoadExists;

            public IClock Clock => central.clock;
            public Time RemainingDuration { get; private set; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get => nextRoadExists ? remainingPath.Current : null; }

            /// <summary>
            /// Creates a new passive navigation between two given coordinates.
            /// </summary>
            /// <param name="central">Central navigation to which the instance will be connected</param>
            public PassiveNavigation(CentralNavigation central, Coords start, Coords finish)
            {
                this.central = central;
                var path = central.map.FindShortestPath(Algorithms.GraphType.NonnegativeWeights, start, finish);
                Debug.Assert(path.TotalWeight < Weight.positiveInfinity);
                RemainingDuration = new Time((int)path.TotalWeight);

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
                RemainingDuration -= new Time((int)CurrentRoad.Weight);
                CurrentRoad = remainingPath.Current;
                nextRoadExists = remainingPath.MoveNext();
            }
        }

        /// <summary>
        /// Represents an active car navigation.
        /// </summary>
        /// <remarks>
        /// Takes into account current traffic situation in the simulation and therefore updates the shortest path
        /// during the car's passage. For efficiency, it uses the central navigation's data to find the current shortest
        /// path, as there may be many navigations querying the same information.
        /// </remarks>
        private class ActiveNavigation : INavigation
        {
            private readonly CentralNavigation central;
            private readonly Coords destination;
            private Time nextRemainingDuration;

            public IClock Clock => central.clock;
            public Time RemainingDuration { get; private set; }
            public Road CurrentRoad { get; private set; }
            public Road NextRoad { get; private set; }

            /// <summary>
            /// Creates a new active navigation between two given coordinates.
            /// </summary>
            /// <param name="central">Central navigation to which the instance will be connected</param>
            public ActiveNavigation(CentralNavigation central, Coords start, Coords finish)
            {
                this.central = central;
                destination = finish;

                var (firstRoad, totalWeight) = central.GetNextRoad(start, finish);
                RemainingDuration = new Time((int)totalWeight);
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
                if (CurrentRoad.Destination.Id == destination)
                    NextRoad = null;
                else
                    UpdateNextRoad();
            }

            /// <summary>
            /// Updates the next road based on current traffic.
            /// </summary>
            private void UpdateNextRoad()
            {
                var (nextRoad, remainingWeight) = central.GetNextRoad(CurrentRoad.ToNode.Id, destination);
                nextRemainingDuration = new Time((int)remainingWeight);
                NextRoad = nextRoad;
            }
        }

        #endregion nested_classes
    }
}
