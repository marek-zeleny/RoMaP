using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents priority crossing rules for a crossroad.
    /// </summary>
    class PriorityCrossing : ICrossingAlgorithm
    {
        /// <summary>
        /// Time reserved for a car to surely go through a crossroad
        /// </summary>
        private static readonly Time maxTimeToCross = 4.Seconds();

        private Dictionary<Direction, DirectionInfo> directions = new();
        private IClock clock;
        private int totalWaitingCars;
        private int currentlyAllowedToGo;
        private Car deadlockAllowedCar;

        public override string ToString()
        {
            return $"PriorCross: {totalWaitingCars} waiting";
        }

        #region building

        public void AddInRoad(Crossroad crossroad, int id)
        {
            foreach (var outRoad in crossroad.GetOutEdges())
                directions.Add(new Direction(id, outRoad.Id), new DirectionInfo());
        }

        public void AddOutRoad(Crossroad crossroad, int id)
        {
            foreach (var inRoad in crossroad.GetInEdges())
                directions.Add(new Direction(inRoad.Id, id), new DirectionInfo());
        }

        public void RemoveInRoad(int id)
        {
            bool Predicate(Direction dir) => dir.fromRoadId == id;

            RemoveDirectionsWhere(Predicate);
            foreach (var info in directions.Values)
                info.RemovePriorDirectionsWhere(Predicate);
        }

        public void RemoveOutRoad(int id)
        {
            bool Predicate(Direction dir) => dir.toRoadId == id;

            RemoveDirectionsWhere(Predicate);
            foreach (var info in directions.Values)
                info.RemovePriorDirectionsWhere(Predicate);
        }

        public void AddPriority(Direction priorityFrom, Direction priorityTo)
        {
            var priors = directions[priorityFrom].PriorDirections;
            if (!priors.Contains(priorityTo))
                priors.Add(priorityTo);
        }

        public void ClearAllPriorities()
        {
            directions = new Dictionary<Direction, DirectionInfo>();
        }

        #endregion building

        #region usage

        /// <summary>
        /// Initialises the priority crossing algorithm before starting a simulation.
        /// </summary>
        /// <param name="clock">Simulation clock necessary for the algorithm</param>
        public void Initialise(IClock clock)
        {
            this.clock = clock;
            totalWaitingCars = 0;
            currentlyAllowedToGo = 0;
            deadlockAllowedCar = null;
            foreach (DirectionInfo info in directions.Values)
                info.Initialise();
        }

        public bool CanCross(Car car, int fromRoadId, int toRoadId, Time expectedArrival)
        {
            Direction dir = new(fromRoadId, toRoadId);
            DirectionInfo info = directions[dir];

            void AddWaitingCar(bool permissionToGo)
            {
                var waiting = info.WaitingCars.FirstOrDefault(w => w.Car == car);
                // If this car is not yet waiting, add it
                if (waiting == default)
                {
                    info.WaitingCars.Add(new(car, clock.Time, permissionToGo));
                    totalWaitingCars++;
                    // Also, new car arrival -> deadlock broken
                    deadlockAllowedCar = null;
                }
                // Otherwise, set permission to go
                else
                {
                    waiting.PermissionToGo = permissionToGo;
                }

                if (permissionToGo)
                    currentlyAllowedToGo++;
            }

            // No priorities to give -> go
            if (info.PriorDirections.Count == 0)
            {
                // Only register the car once it's near the crossroad
                if (expectedArrival < maxTimeToCross)
                    AddWaitingCar(true);
                return true;
            }
            // Possibly need to give priority and not yet at the crossroad -> stop
            if (expectedArrival > 0)
                return false;

            // Already waiting at the crossroad
            // Deadlock detected -> allow through
            if (car == deadlockAllowedCar)
            {
                AddWaitingCar(true);
                return true;
            }
            // General situation -> check priorities
            foreach (Direction prior in info.PriorDirections)
                if (directions[prior].WaitingCars.Count > 0)
                {
                    AddWaitingCar(false);
                    return false;
                }
            AddWaitingCar(true);
            return true;
        }

        public void CarCrossed(Car car, int fromRoadId, int toRoadId)
        {
            Direction dir = new(fromRoadId, toRoadId);
            var waitingCars = directions[dir].WaitingCars;
            var waiting = waitingCars.First(w => w.Car == car); // Should always be found, so throwing exception is OK
            Debug.Assert(waiting.PermissionToGo);
            totalWaitingCars--;
            waitingCars.Remove(waiting);
        }

        public void Tick(Time _)
        {
            if (totalWaitingCars > 0 && currentlyAllowedToGo == 0)
            {
                // Priority deadlock detected -> find the longest waiting car and allow it to go
                DirectionInfo.WaitingCar oldest = null;
                foreach (var (_, info) in directions)
                    foreach (var waiting in info.WaitingCars)
                        if (oldest == null || waiting.Arrival < oldest.Arrival)
                            oldest = waiting;
                deadlockAllowedCar = oldest.Car;
            }
            else
            {
                deadlockAllowedCar = null;
            }
            currentlyAllowedToGo = 0;
        }

        /// <summary>
        /// Removes all information about directions satisfying a given predicate.
        /// </summary>
        private void RemoveDirectionsWhere(Func<Direction, bool> predicate)
        {
            var keys = directions.Keys.Where(predicate).ToList();
            foreach (var key in keys)
            {
                bool result = directions.Remove(key);
                Debug.Assert(result);
            }
        }

        #endregion usage

        /// <summary>
        /// Contains information about a certain direction.
        /// </summary>
        private class DirectionInfo
        {
            /// <summary>
            /// Contains information about a car asking for permission to go through the crossroad.
            /// </summary>
            public class WaitingCar
            {
                public Car Car { get; set; }
                /// <summary>
                /// Time at which the car began waiting for permission
                /// </summary>
                public Time Arrival { get; set; }
                /// <summary>
                /// <c>true</c> if a permission to go was given, otherwise <c>false</c>
                /// </summary>
                public bool PermissionToGo { get; set; }

                public WaitingCar(Car car, Time arrival, bool permissionToGo)
                {
                    Car = car;
                    Arrival = arrival;
                    PermissionToGo = permissionToGo;
                }
            }

            private List<Direction> priorDirections = new();

            /// <summary>
            /// Collection of directions this direction must give priority to
            /// </summary>
            public ICollection<Direction> PriorDirections { get => priorDirections; }
            /// <summary>
            /// Set of all cars waiting for (or already having) permission to go in this direction.
            /// </summary>
            public ICollection<WaitingCar> WaitingCars { get; private set; }

            /// <summary>
            /// Initialises the direction information before starting a simulation.
            /// </summary>
            public void Initialise()
            {
                WaitingCars = new List<WaitingCar>(Road.maxLaneCount); // Capacity ~ number of lanes of a road
            }

            /// <summary>
            /// Removes all prior directions satisfying a given predicate.
            /// </summary>
            public void RemovePriorDirectionsWhere(Func<Direction, bool> predicate)
            {
                priorDirections.RemoveAll(dir => predicate(dir));
            }

            public override string ToString() => $"Prior: {priorDirections.Count}, waiting: {WaitingCars.Count}";
        }
    }
}
