using System;
using System.Collections.Generic;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents a crossroad in a map.
    /// </summary>
    class Crossroad : Node<Coords, int>
    {
        private byte carSpawnRate = 10;

        /// <summary>
        /// Traffic light settings at the crossroad
        /// </summary>
        public TrafficLight TrafficLight { get; }
        /// <summary>
        /// Priority crossing settings at the crossroad
        /// </summary>
        public PriorityCrossing PriorityCrossing { get; }
        /// <summary>
        /// Currently active crossing algorithm at the crossroad
        /// </summary>
        public ICrossingAlgorithm ActiveCrossingAlgorithm { get; private set; }
        /// <summary>
        /// Rate of spawning new cars at this crossroad relative to other crossroads (value between 0 and 100)
        /// </summary>
        public byte CarSpawnRate
        {
            get => carSpawnRate;
            set
            {
                if (value > 100)
                    carSpawnRate = 100;
                else
                    carSpawnRate = value;
            }
        }

        /// <summary>
        /// Creates a new crossroad with a given ID.
        /// </summary>
        public Crossroad(Coords id)
            : base(id)
        {
            TrafficLight = new TrafficLight();
            PriorityCrossing = new PriorityCrossing();
            ActiveCrossingAlgorithm = PriorityCrossing;
        }

        /// <summary>
        /// Activates traffic lights at the crossroad.
        /// </summary>
        public void ActivateTrafficLight()
        {
            ActiveCrossingAlgorithm = TrafficLight;
        }

        /// <summary>
        /// Deactivates traffic lights at the crossroad.
        /// </summary>
        public void DeactivateTrafficLight()
        {
            ActiveCrossingAlgorithm = PriorityCrossing;
        }

        /// <summary>
        /// Performs necessary initial actions and checks before starting a simulation.
        /// </summary>
        /// <param name="clock">Simulation clock necessary for the crossroad's functioning</param>
        /// <returns><c>true</c> if all checks are successful, otherwise <c>false</c></returns>
        public bool Initialise(IClock clock)
        {
            if (ActiveCrossingAlgorithm == PriorityCrossing)
            {
                PriorityCrossing.Initialise(clock);
                return true;
            }
            else
            {
                Debug.Assert(ActiveCrossingAlgorithm == TrafficLight);
                if (TrafficLight.Settings.Count > 1)
                    ActiveCrossingAlgorithm = TrafficLight;
                else
                    ActiveCrossingAlgorithm = PriorityCrossing;

                var trafficLightVerifier = new Dictionary<Direction, bool>(InDegree * OutDegree);
                foreach (Road inRoad in GetInEdges())
                    foreach (Road outRoad in GetOutEdges())
                        trafficLightVerifier.Add(new Direction(inRoad.Id, outRoad.Id), false);
                return TrafficLight.Initialise(trafficLightVerifier);
            }
        }

        /// <summary>
        /// Performs a simulation step of a given duration on the crossroad.
        /// </summary>
        public void Tick(Time time)
        {
            ActiveCrossingAlgorithm.Tick(time);
        }

        public override void AddInEdge(IEdge<Coords, int> edge)
        {
            base.AddInEdge(edge);
            PriorityCrossing.AddInRoad(this, edge.Id);
        }

        public override void AddOutEdge(IEdge<Coords, int> edge)
        {
            base.AddOutEdge(edge);
            PriorityCrossing.AddOutRoad(this, edge.Id);
        }

        public override bool RemoveInEdge(int id)
        {
            bool result = base.RemoveInEdge(id);
            if (result)
            {
                TrafficLight.RemoveRoad(id);
                PriorityCrossing.RemoveInRoad(id);
            }
            return result;
        }

        public override bool RemoveOutEdge(int id)
        {
            bool result = base.RemoveOutEdge(id);
            if (result)
            {
                TrafficLight.RemoveRoad(id);
                PriorityCrossing.RemoveOutRoad(id);
            }
            return result;
        }
    }
}
