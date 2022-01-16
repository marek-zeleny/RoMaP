using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    static class MapSaverLoader
    {
        private static class Keywords
        {
            public const string sideOfDriving = "sideOfDriving";
            public const string right = "right";
            public const string left = "left";

            public const string roads = "roads";
            public const string crossroads = "crossroads";

            public const string forward = "forward";
            public const string backward = "backward";
            public const string route = "route";

            public const string open = "open";
            public const string id = "id";
            public const string length = "length";
            public const string maxSpeed = "maxSpeed";
            public const string laneCount = "laneCount";

            public const string mainRoadDirections = "mainRoadDirections";
            public const string carSpawnRate = "carSpawnRate";
            public const string trafficLightSettings = "trafficLightSettings";

            public const string duration = "duration";
            public const string allowedDirections = "allowedDirections";

            public const string x = "x";
            public const string y = "y";

            public const string from = "from";
            public const string to = "to";
        }

        public static void SaveMap(Stream stream, Map map, IGMap guiMap)
        {
            JsonWriterOptions options = new()
            {
                Indented = true,
            };
            Utf8JsonWriter writer = new(stream, options);
            writer.WriteStartObject();

            string sideOfDriving = MapManager.roadSide switch
            {
                MapManager.RoadSide.Right => Keywords.right,
                MapManager.RoadSide.Left => Keywords.left,
                _ => throw new NotImplementedException(),
            };
            writer.WriteString(Keywords.sideOfDriving, sideOfDriving);

            writer.WriteStartArray(Keywords.roads);
            foreach (var gRoad in guiMap.GetRoads())
                SerialiseRoad(writer, gRoad);
            writer.WriteEndArray();

            writer.WriteStartArray(Keywords.crossroads);
            foreach (var gCrossroad in guiMap.GetCrossroads())
            {
                Crossroad crossroad = (Crossroad)map.GetNode(gCrossroad.CrossroadId);
                SerialiseCrossroad(writer, gCrossroad, crossroad);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.Flush();
        }

        public static bool LoadMap(Stream stream, Map map, IGMap guiMap)
        {
            // TODO: support side of driving
            using var document = JsonDocument.Parse(stream);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return false;
            var elements = root.EnumerateObject();
            // Load roads
            if (!elements.MoveNext())
                return false;
            if (elements.Current.Name != Keywords.roads)
                return false;
            var roads = elements.Current.Value;
            if (roads.ValueKind != JsonValueKind.Array)
                return false;
            Dictionary<int, int> roadIdMapper = new();
            foreach (var road in roads.EnumerateArray())
            {
                if (!ParseRoad(road, map, guiMap, out var roadIdMappings))
                    return false;
                foreach (var mapping in roadIdMappings)
                    roadIdMapper.Add(mapping.previous, mapping.current);
            }
            // Load crossroads
            if (!elements.MoveNext())
                return false;
            if (elements.Current.Name != Keywords.crossroads)
                return false;
            var crossroads = elements.Current.Value;
            if (crossroads.ValueKind != JsonValueKind.Array)
                return false;
            foreach (var crossroad in crossroads.EnumerateArray())
            {
                if (!ParseCrossroad(crossroad, map, guiMap, roadIdMapper))
                    return false;
            }
            // No other element should be present
            return !elements.MoveNext();
        }

        private static void SerialiseRoad(Utf8JsonWriter writer, IGRoad gRoad)
        {
            void Serialise(Road road)
            {
                writer.WriteStartObject();
                if (road.IsConnected)
                {
                    writer.WriteBoolean(Keywords.open, true);
                    writer.WriteNumber(Keywords.id, road.Id);
                }
                else
                {
                    writer.WriteBoolean(Keywords.open, false);
                }
                writer.WriteNumber(Keywords.length, road.Length.ToMetres());
                writer.WriteNumber(Keywords.maxSpeed, road.MaxSpeed.ToKilometresPerHour());
                writer.WriteNumber(Keywords.laneCount, road.LaneCount);
                writer.WriteEndObject();
            }

            writer.WriteStartObject();

            Road road = gRoad.GetRoad(IGRoad.Direction.Forward);
            if (road != null)
            {
                writer.WritePropertyName(Keywords.forward);
                Serialise(road);
            }
            road = gRoad.GetRoad(IGRoad.Direction.Backward);
            if (road != null)
            {
                writer.WritePropertyName(Keywords.backward);
                Serialise(road);
            }
            writer.WriteStartArray(Keywords.route);
            foreach (var coords in gRoad.GetRoute())
                SerialiseCoords(writer, coords);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private static void SerialiseCrossroad(Utf8JsonWriter writer, IGCrossroad gCrossroad, Crossroad crossroad)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(Keywords.id);
            SerialiseCoords(writer, crossroad.Id);
            if (gCrossroad.MainRoadDirections.HasValue)
            {
                writer.WriteStartArray(Keywords.mainRoadDirections);
                SerialiseCoords(writer, gCrossroad.MainRoadDirections.Value.Item1);
                SerialiseCoords(writer, gCrossroad.MainRoadDirections.Value.Item2);
                writer.WriteEndArray();
            }
            writer.WriteNumber(Keywords.carSpawnRate, crossroad.CarSpawnRate);

            writer.WriteStartArray(Keywords.trafficLightSettings);
            foreach (var setting in crossroad.TrafficLight.Settings)
            {
                writer.WriteStartObject();
                writer.WriteNumber(Keywords.duration, setting.Duration.ToSeconds());
                writer.WriteStartArray(Keywords.allowedDirections);
                foreach (var direction in setting)
                    SerialiseDirection(writer, direction);
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private static void SerialiseCoords(Utf8JsonWriter writer, Coords coords)
        {
            writer.WriteStartObject();
            writer.WriteNumber(Keywords.x, coords.x);
            writer.WriteNumber(Keywords.y, coords.y);
            writer.WriteEndObject();
        }

        private static void SerialiseDirection(Utf8JsonWriter writer, Direction direction)
        {
            writer.WriteStartObject();
            writer.WriteNumber(Keywords.from, direction.fromRoadId);
            writer.WriteNumber(Keywords.to, direction.toRoadId);
            writer.WriteEndObject();
        }

        private static bool ParseRoad(JsonElement jRoad, Map map, IGMap gMap,
            out IEnumerable<(int previous, int current)> roadIdMappings)
        {
            // TODO: support closed roads
            static bool Parse(JsonElement jRoad, Road road, ref (int previous, int current) idMapping)
            {
                if (!ParseIntProperty(jRoad, Keywords.id, out int id))
                    return false;
                if (!ParseIntProperty(jRoad, Keywords.length, out int length))
                    return false;
                if (!ParseIntProperty(jRoad, Keywords.maxSpeed, out int maxSpeed))
                    return false;
                if (!ParseIntProperty(jRoad, Keywords.laneCount, out int laneCount))
                    return false;
                road.Length = length.Metres();
                road.MaxSpeed = maxSpeed.KilometresPerHour();
                road.LaneCount = laneCount;
                idMapping.previous = id;
                idMapping.current = road.Id;
                return true;
            }

            roadIdMappings = null;
            if (jRoad.ValueKind != JsonValueKind.Object)
                return false;

            bool forward = jRoad.TryGetProperty(Keywords.forward, out var jForward);
            bool backward = jRoad.TryGetProperty(Keywords.backward, out var jBackward);
            if (!forward && !backward)
                return false;
            if (!jRoad.TryGetProperty(Keywords.route, out var jRoute))
                return false;

            if (jRoute.ValueKind != JsonValueKind.Array)
                return false;
            var routeEnum = jRoute.EnumerateArray().GetEnumerator();
            if (!routeEnum.MoveNext())
                return false;
            if (!ParseCoords(routeEnum.Current, out Coords firstCoords))
                return false;
            IRoadBuilder builder = MapManager.CreateRoadBuilder(map, gMap, firstCoords, forward && backward);
            if (builder == null)
                return false;
            while (routeEnum.MoveNext())
            {
                if (!ParseCoords(routeEnum.Current, out Coords coords)
                        || !builder.AddSegment(coords))
                {
                    builder.DestroyRoad();
                    return false;
                }
            }
            if (!builder.FinishRoad(out IGRoad gRoad))
            {
                builder.DestroyRoad();
                return false;
            }

            int roadCount = forward && backward ? 2 : 1;
            var idMappings = new (int, int)[roadCount];
            roadIdMappings = idMappings;
            if (forward)
                if (!Parse(jForward, gRoad.GetRoad(IGRoad.Direction.Forward) as Road, ref idMappings[0]))
                    return false;
            if (backward)
                if (!Parse(jBackward, gRoad.GetRoad(IGRoad.Direction.Backward) as Road, ref idMappings[roadCount - 1]))
                    return false;
            return true;
        }

        private static bool ParseCrossroad(JsonElement jCrossroad, Map map, IGMap gMap,
            Dictionary<int, int> roadIdMapper)
        {
            bool ParseTrafficLightSetting(JsonElement jSetting, TrafficLight.Setting setting)
            {
                if (jSetting.ValueKind != JsonValueKind.Object)
                    return false;
                if (!ParseIntProperty(jSetting, Keywords.duration, out int duration))
                    return false;
                setting.Duration = duration.Seconds();

                if (!jSetting.TryGetProperty(Keywords.allowedDirections, out var jDirections))
                    return false;
                if (jDirections.ValueKind != JsonValueKind.Array)
                    return false;
                foreach (var jDir in jDirections.EnumerateArray())
                {
                    if (!ParseDirection(jDir, out var dir))
                        return false;
                    if (!roadIdMapper.TryGetValue(dir.fromRoadId, out int from)
                        || !roadIdMapper.TryGetValue(dir.toRoadId, out int to))
                        return false;
                    setting.AddDirection(from, to);
                }
                return true;
            }

            if (jCrossroad.ValueKind != JsonValueKind.Object)
                return false;

            if (!jCrossroad.TryGetProperty(Keywords.id, out var jId))
                return false;
            if (!ParseCoords(jId, out Coords id))
                return false;
            Crossroad crossroad = (Crossroad)map.GetNode(id);
            if (crossroad == null)
                return false;

            if (jCrossroad.TryGetProperty(Keywords.mainRoadDirections, out var jMainRoad))
            {
                if (jMainRoad.ValueKind != JsonValueKind.Array)
                    return false;
                var e = jMainRoad.EnumerateArray();
                if (!e.MoveNext())
                    return false;
                if (!ParseCoords(e.Current, out Coords dir1))
                    return false;
                if (!e.MoveNext())
                    return false;
                if (!ParseCoords(e.Current, out Coords dir2))
                    return false;
                // Shouldn't have more elements
                if (e.MoveNext())
                    return false;
                IGCrossroad gCrossroad = gMap.GetCrossroad(id);
                Debug.Assert(gCrossroad != null);
                gCrossroad.MainRoadDirections = (dir1, dir2);
            }

            if (!ParseIntProperty(jCrossroad, Keywords.carSpawnRate, out int carSpawnRate))
                return false;
            if (carSpawnRate < byte.MinValue || carSpawnRate > byte.MaxValue)
                return false;
            crossroad.CarSpawnRate = (byte)carSpawnRate;

            if (!jCrossroad.TryGetProperty(Keywords.trafficLightSettings, out var jTrafficLight))
                return false;
            if (jTrafficLight.ValueKind != JsonValueKind.Array)
                return false;
            var settingsEnum = jTrafficLight.EnumerateArray().GetEnumerator();
            TrafficLight trafficLight = crossroad.TrafficLight;
            // First setting is already created
            if (!settingsEnum.MoveNext())
                return false;
            if (!ParseTrafficLightSetting(settingsEnum.Current, trafficLight.Settings[0]))
                return false;
            while (settingsEnum.MoveNext())
            {
                var setting = trafficLight.AddSetting();
                if (setting == null)
                    return false;
                if (!ParseTrafficLightSetting(settingsEnum.Current, setting))
                    return false;
            }
            return true;
        }

        static bool ParseIntProperty(JsonElement elem, string propertyName, out int value)
        {
            value = default;
            if (!elem.TryGetProperty(propertyName, out var jValue))
                return false;
            if (jValue.ValueKind != JsonValueKind.Number)
                return false;
            return jValue.TryGetInt32(out value);
        }

        private static bool ParseCoords(JsonElement jCoords, out Coords coords)
        {
            coords = default;
            if (jCoords.ValueKind != JsonValueKind.Object)
                return false;
            if (!ParseIntProperty(jCoords, Keywords.x, out int x))
                return false;
            if (!ParseIntProperty(jCoords, Keywords.y, out int y))
                return false;
            coords = new Coords(x, y);
            return true;
        }

        private static bool ParseDirection(JsonElement jDirection, out Direction direction)
        {
            direction = default;
            if (jDirection.ValueKind != JsonValueKind.Object)
                return false;
            if (!ParseIntProperty(jDirection, Keywords.from, out int from))
                return false;
            if (!ParseIntProperty(jDirection, Keywords.to, out int to))
                return false;
            direction = new Direction(from, to);
            return true;
        }
    }
}
