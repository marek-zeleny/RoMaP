using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    /// <summary>
    /// Provides methods for saving and loading maps.
    /// </summary>
    static class MapSaverLoader
    {
        /// <summary>
        /// String constants used in saved files.
        /// </summary>
        /// <remarks>
        /// All strings used in the files should be defined here; do not use string literals in the code.
        /// </remarks>
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

        private static readonly JsonWriterOptions writerOptions = new() { Indented = true };

        #region interface

        /// <summary>
        /// Saves a given map (back-end and graphical) as a JSON document using a given stream.
        /// </summary>
        /// <remarks>
        /// If the given back-end map and GUI map are not consistent with respect to each other, the behaviour is
        /// undefined.
        /// </remarks>
        public static void SaveMap(Stream stream, Map map, IGMap guiMap)
        {
            Utf8JsonWriter writer = new(stream, writerOptions);
            writer.WriteStartObject();

            string sideOfDriving = guiMap.SideOfDriving switch
            {
                RoadSide.Right => Keywords.right,
                RoadSide.Left => Keywords.left,
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
                // May be closed crossroad (only GUI) - need to check
                if (crossroad != null)
                    SerialiseCrossroad(writer, gCrossroad, crossroad);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.Flush();
        }

        /// <summary>
        /// Loads a saved map from a given stream and stores it into given back-end and graphical map objects.
        /// </summary>
        /// <remarks>
        /// The method assumes that given map objects are empty (i.e. in their initial state).
        /// 
        /// Note that the created map may not be internally identical to the original one that was saved, as IDs of
        /// roads can change during the process. However, this change is only an implementation detail and is not
        /// observable from the outside.
        /// </remarks>
        /// <param name="map">
        /// Back-end map object to which the map was loaded; if loading failed, the map's state is undefined
        /// </param>
        /// <param name="guiMap">
        /// Graphical map object to which the map was loaded; if loading failed, the map's state is undefined
        /// </param>
        /// <returns><c>true</c> if the map was successfully loaded, otherwise <c>false</c></returns>
        public static bool LoadMap(Stream stream, Map map, IGMap guiMap)
        {
            using var document = JsonDocument.Parse(stream);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return false;
            var elements = root.EnumerateObject();
            // Load side of driving
            if (!elements.MoveNext())
                return false;
            if (!ParseStringProperty(elements.Current, Keywords.sideOfDriving, out string side))
                return false;
            switch (side)
            {
                case Keywords.right:
                    guiMap.SideOfDriving = RoadSide.Right;
                    break;
                case Keywords.left:
                    guiMap.SideOfDriving = RoadSide.Left;
                    break;
                default:
                    return false;
            }
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
                foreach (var (previous, current) in roadIdMappings)
                    roadIdMapper.Add(previous, current);
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

        #endregion interface

        #region serialising

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
                writer.WriteStringValue(gCrossroad.MainRoadDirections.Value.Item1.ToString());
                writer.WriteStringValue(gCrossroad.MainRoadDirections.Value.Item2.ToString());
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

        #endregion serialising

        #region highLevelParsers

        /// <summary>
        /// Parses a road from a given JSON element and adds it to the given map objects.
        /// </summary>
        /// <remarks>
        /// If the parsing was not successful, states of the map objects are undefined.
        /// 
        /// Note that the parsed road may have a different ID than the one in the JSON file. Use the outputted road ID
        /// mappings to resolve potential inconsistencies.
        /// </remarks>
        /// <param name="roadIdMappings">
        /// Outputs mappings of road IDs from the JSON file to actual IDs of the created roads (see remarks for
        /// details); if parsing failed, the contents are undefined
        /// </param>
        /// <returns><c>true</c> if the road was successfully parsed, otherwise <c>false</c></returns>
        private static bool ParseRoad(JsonElement jRoad, Map map, IGMap gMap,
            out IEnumerable<(int previous, int current)> roadIdMappings)
        {
            bool Parse(JsonElement jRoad, IGRoad gRoad, IGRoad.Direction direction, ref (int prev, int curr)? idMapping)
            {
                Road road = gRoad.GetRoad(direction);
                if (!ParseBooleanProperty(jRoad, Keywords.open, out bool open))
                    return false;
                if (open)
                {
                    // Road open -> load previous ID and map it to the new one
                    if (!ParseIntProperty(jRoad, Keywords.id, out int id))
                        return false;
                    idMapping = (id, road.Id);
                }
                else
                {
                    // Road closed -> remove from map, set highlight, etc.
                    map.RemoveRoad(road.Id);
                    gRoad.SetHighlight(Highlight.Transparent, direction);
                }
                if (!ParseIntProperty(jRoad, Keywords.length, out int length))
                    return false;
                if (!ParseIntProperty(jRoad, Keywords.maxSpeed, out int maxSpeed))
                    return false;
                if (!ParseIntProperty(jRoad, Keywords.laneCount, out int laneCount))
                    return false;
                road.Length = length.Metres();
                road.MaxSpeed = maxSpeed.KilometresPerHour();
                road.LaneCount = laneCount;
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
            IRoadBuilder builder = MapManager.CreateRoadBuilder(map, gMap, firstCoords);
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
            if (!builder.FinishRoad(forward && backward, out IGRoad gRoad, false))
            {
                builder.DestroyRoad();
                return false;
            }

            var idMappings = new (int, int)?[2];
            if (forward)
                if (!Parse(jForward, gRoad, IGRoad.Direction.Forward, ref idMappings[0]))
                    return false;
            if (backward)
                if (!Parse(jBackward, gRoad, IGRoad.Direction.Backward, ref idMappings[1]))
                    return false;
            roadIdMappings = idMappings.Where(nullable => nullable.HasValue).Select(nullable => nullable.Value);
            return true;
        }

        /// <summary>
        /// Parses a crossroad from a given JSON element and adds it to the given map objects.
        /// </summary>
        /// <remarks>
        /// If the parsing was not successful, states of the map objects are undefined.
        /// </remarks>
        /// <param name="roadIdMapper">Mappings of road IDs from the JSON file to IDs of the actual roads</param>
        /// <returns><c>true</c> if the crossroad was successfully parsed, otherwise <c>false</c></returns>
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
                if (e.Current.ValueKind != JsonValueKind.String)
                    return false;
                if (!Enum.TryParse(e.Current.GetString(), out CoordsConvertor.Direction dir1))
                    return false;
                if (!e.MoveNext())
                    return false;
                if (e.Current.ValueKind != JsonValueKind.String)
                    return false;
                if (!Enum.TryParse(e.Current.GetString(), out CoordsConvertor.Direction dir2))
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

        #endregion highLevelParsers

        #region lowLevelParsers

        /// <summary>
        /// Parses coordinates from a given JSON element.
        /// </summary>
        /// <param name="coords">Outputs the parsed coordinates; if parsing failed, the value is undefined</param>
        /// <returns><c>true</c> if the coordinates were successfully parsed, otherwise <c>false</c></returns>
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

        /// <summary>
        /// Parses a direction (from one road to another) from a given JSON element.
        /// </summary>
        /// <param name="direction">Outputs the parsed direction; if parsing failed, the value is undefined</param>
        /// <returns><c>true</c> if the direction was successfully parsed, otherwise <c>false</c></returns>
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

        /// <summary>
        /// Parses string value of a given property.
        /// </summary>
        /// <param name="propertyName">Expected name of the property; if not consistent, parsing fails</param>
        /// <param name="value">Outputs the parsed string value; if parsing failed, the value is undefined</param>
        /// <returns><c>true</c> if the property was successfully parsed, otherwise <c>false</c></returns>
        private static bool ParseStringProperty(JsonProperty prop, string propertyName, out string value)
        {
            value = default;
            if (prop.Name != propertyName)
                return false;
            if (prop.Value.ValueKind != JsonValueKind.String)
                return false;
            value = prop.Value.GetString();
            return true;
        }

        /// <summary>
        /// Parses integer value of a property with a given name in a given JSON element.
        /// </summary>
        /// <param name="value">Outputs the parsed integer value; if parsing failed, the value is undefined</param>
        /// <returns><c>true</c> if the property was successfully parsed, otherwise <c>false</c></returns>
        private static bool ParseIntProperty(JsonElement elem, string propertyName, out int value)
        {
            value = default;
            if (!elem.TryGetProperty(propertyName, out var jValue))
                return false;
            if (jValue.ValueKind != JsonValueKind.Number)
                return false;
            return jValue.TryGetInt32(out value);
        }

        /// <summary>
        /// Parses boolean value of a property with a given name in a given JSON element.
        /// </summary>
        /// <param name="value">Outputs the parsed boolean value; if parsing failed, the value is undefined</param>
        /// <returns><c>true</c> if the property was successfully parsed, otherwise <c>false</c></returns>
        private static bool ParseBooleanProperty(JsonElement elem, string propertyName, out bool value)
        {
            value = default;
            if (!elem.TryGetProperty(propertyName, out var jValue))
                return false;
            if (jValue.ValueKind == JsonValueKind.True)
            {
                value = true;
                return true;
            }
            else if (jValue.ValueKind == JsonValueKind.False)
            {
                value = false;
                return true;
            }
            else
                return false;
        }

        #endregion lowLevelParsers
    }
}
