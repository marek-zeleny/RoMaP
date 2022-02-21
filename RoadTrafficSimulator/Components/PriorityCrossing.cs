using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    class PriorityCrossing : ICrossingAlgorithm
    {
        private Dictionary<Direction, DirectionInfo> directions = new();
        private IClock clock;

        public void Initialise(IClock clock)
        {
            this.clock = clock;
            foreach (DirectionInfo info in directions.Values)
                info.Initialise();
        }

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

        public bool CanCross(Car car, int fromRoadId, int toRoadId, Time expectedArrival)
        {
            Direction dir = new(fromRoadId, toRoadId);
            DirectionInfo info = directions[dir];

            void AddWaitingCar()
            {
                // If this car is already waiting, do nothing
                if (info.WaitingCars.Where(w => w.car == car).Any())
                    return;
                info.WaitingCars.Add(new(car, toRoadId, clock.Time));
            }

            // No priorities to give -> go
            if (info.PriorDirections.Count == 0)
            {
                AddWaitingCar();
                return true;
            }
            // Possibly need to give priority and not yet at the crossroad -> stop
            if (expectedArrival > 0)
                return false;
            // Already waiting at the crossroad -> decide
            AddWaitingCar();
            foreach (Direction prior in info.PriorDirections)
                if (directions[prior].WaitingCars.Count > 0)
                    return false;
            return true;
        }

        public void CarCrossed(Car car, int fromRoadId, int toRoadId)
        {
            Direction dir = new(fromRoadId, toRoadId);
            var waitingCars = directions[dir].WaitingCars;
            var waiting = waitingCars.First(w => w.car == car); // Should always be found, so throwing exception is OK
            Debug.Assert(waiting.toRoadId == toRoadId);
            waitingCars.Remove(waiting);
        }

        private void RemoveDirectionsWhere(Func<Direction, bool> predicate)
        {
            var keys = directions.Keys.Where(predicate).ToList();
            foreach (var key in keys)
            {
                bool result = directions.Remove(key);
                Debug.Assert(result);
            }
        }

        private class DirectionInfo
        {
            private List<Direction> priorDirections = new();

            public ICollection<Direction> PriorDirections { get => priorDirections; }
            public ICollection<(Car car, int toRoadId, Time arrival)> WaitingCars { get; private set; }

            public void Initialise()
            {
                WaitingCars = new List<(Car, int, Time)>();
            }

            public void RemovePriorDirectionsWhere(Func<Direction, bool> predicate)
            {
                priorDirections.RemoveAll(dir => predicate(dir));
            }

            public override string ToString() => $"Prior: {priorDirections.Count}, waiting: {WaitingCars.Count}";
        }
    }
}
