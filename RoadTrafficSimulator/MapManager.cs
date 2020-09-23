using System;
using System.Collections.Generic;
using System.Drawing;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class MapManager
    {
        private Components.Map map = new Components.Map();
        private IMap guiMap = new GUI.Map();
        private GuiSettings settings;
        private RoadBuilder builder = null;

        public bool IsBuilding { get => builder == null; }

        public void Draw(Graphics graphics, int width, int height)
        {
            DrawGrid(graphics, width, height);
            guiMap.Draw(graphics, settings.origin, settings.zoom, width, height);
        }

        public void StartBuildingRoad(Coords startingCoords, bool twoWayRoad = true)
        {
            if (IsBuilding)
                throw new InvalidOperationException("A road is already being built.");
            builder = new RoadBuilder(startingCoords, twoWayRoad);
        }

        public void BuildRoadSegment(Coords nextCoords)
        {
            if (!IsBuilding)
                throw new InvalidOperationException("No road is being built.");
        }

        public void FinishBuildingRoad()
        {
            if (!IsBuilding)
                throw new InvalidOperationException("No road is being built.");
        }

        private void DrawGrid(Graphics graphics, int width, int height)
        {

        }

        private struct GuiSettings
        {
            public Coords origin;
            public decimal zoom;
        }

        private class RoadBuilder
        {
            private Coords startingCoords;
            private bool twoWayRoad;

            public RoadBuilder(Coords startingCoords, bool twoWayRoad)
            {
                this.startingCoords = startingCoords;
                this.twoWayRoad = twoWayRoad;
            }
        }
    }
}
