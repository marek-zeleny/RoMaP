using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoadTrafficSimulator.Components
{
    class PriorityCrossing : ICrossingAlgorithm
    {
        private Dictionary<Direction, DirectionInfo> directions;

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
            Func<Direction, bool> predicate = dir => dir.fromRoadId == id;
            RemoveDirectionsWhere(predicate);
            foreach (var info in directions.Values)
                info.RemovePriorDirectionsWhere(predicate);
        }

        public void RemoveOutRoad(int id)
        {
            Func<Direction, bool> predicate = dir => dir.toRoadId == id;
            RemoveDirectionsWhere(predicate);
            foreach (var info in directions.Values)
                info.RemovePriorDirectionsWhere(predicate);
        }

        public bool CanCross(int fromRoadId, int toRoadId)
        {
            Direction dir = new Direction(fromRoadId, toRoadId);
            DirectionInfo info = directions[dir];
            info.waitingCars++;
            foreach (Direction prior in info.priorDirections)
                if (directions[prior].waitingCars > 0)
                    return false;
            return true;
        }

        public void CarCrossed(int fromRoadId, int toRoadId)
        {
            Direction dir = new Direction(fromRoadId, toRoadId);
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
            public LinkedList<Direction> priorDirections = new LinkedList<Direction>();

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
