using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoadTrafficSimulator.Components
{
    class PriorityCrossing : ICrossingAlgorithm
    {
        private Dictionary<Direction, DirectionInfo> directions = new();

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
            var priors = directions[priorityFrom].priorDirections;
            if (!priors.Contains(priorityTo))
                priors.AddLast(priorityTo);
        }

        public void ClearAllPriorities()
        {
            directions = new Dictionary<Direction, DirectionInfo>();
        }

        public bool CanCross(int fromRoadId, int toRoadId)
        {
            Direction dir = new(fromRoadId, toRoadId);
            DirectionInfo info = directions[dir];
            info.waitingCars++;
            foreach (Direction prior in info.priorDirections)
                if (directions[prior].waitingCars > 0)
                    return false;
            return true;
        }

        public void CarCrossed(int fromRoadId, int toRoadId)
        {
            Direction dir = new(fromRoadId, toRoadId);
            directions[dir].waitingCars--;
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
            public int waitingCars = 0;
            public LinkedList<Direction> priorDirections = new();

            public void RemovePriorDirectionsWhere(Func<Direction, bool> predicate)
            {
                var curr = priorDirections.First;
                while (curr != null)
                {
                    var next = curr.Next;
                    if (predicate(curr.Value))
                        priorDirections.Remove(curr);
                    curr = next;
                }
            }

            public override string ToString() => $"Prior: {priorDirections.Count}, waiting: {waitingCars}";
        }
    }
}
