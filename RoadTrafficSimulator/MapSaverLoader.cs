using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    static class MapSaverLoader
    {
        private const string keywordRoads = "===ROADS===";
        private const string keywordCrossroads = "===CROSSROADS===";
        private const string keywordEnd = "===END===";
        private const char fieldSeparator = '|';
        private const char entitySeparator = ';';
        private const char valueSeparator = ',';
        private const char commentMark = '#';

        /**
         * Format:
         * (without any spaces, they are added just for visual clarity)
         * 
         * ===ROADS===
         * roadId [| backRoadId] | maxSpeed | path
         * ...
         * ===CROSSROADS===
         * coords | carSpawnRate [| settings1 [| settings2] ...]
         * ...
         * ===END===
         * 
         * where
         * path: coords1 ; coords2 [; coords3 [; coords4] ...]
         * coords: x , y
         * settings: duration ; direction1 [; direction2 [; direction3] ...]
         * direction: from , to
         */

        public static void SaveMap(Stream stream, Map map, IGMap guiMap)
        {
            JsonWriterOptions options = new()
            {
                Indented = true,
            };
            Utf8JsonWriter writer = new(stream, options);
            writer.WriteStartObject();

            writer.WriteStartArray("roads");
            foreach (var gRoad in guiMap.GetRoads())
                SerialiseRoad(writer, gRoad);
            writer.WriteEndArray();

            writer.WriteStartArray("crossroads");
            foreach (var gCrossroad in guiMap.GetCrossroads())
            {
                Crossroad crossroad = (Crossroad)map.GetNode(gCrossroad.CrossroadId);
                SerialiseCrossroad(writer, gCrossroad, crossroad);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
            writer.Flush();
        }

        public static bool LoadMap(TextReader reader, Map map, IGMap guiMap)
        {
            string line;
            // Find the beginning of roads
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (IgnoreLine(line))
                    continue;
                else if (line == keywordRoads)
                    break;
                else
                    return false;
            }
            // Load roads
            Dictionary<int, int> roadIdMapper = new Dictionary<int, int>();
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (IgnoreLine(line))
                    continue;
                else if (line == keywordCrossroads)
                    break;
                else if (ParseRoad(line, map, guiMap, out var roadIdMappings))
                    foreach (var (key, value) in roadIdMappings)
                        roadIdMapper.Add(key, value);
                else
                    return false;
            }
            // Load crossroads
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (IgnoreLine(line))
                    continue;
                else if (line == keywordEnd)
                    return true;
                else if (!ParseCrossroad(line, map, roadIdMapper))
                    return false;
            }
            return false;
        }

        private static void SerialiseRoad(Utf8JsonWriter writer, IGRoad gRoad)
        {
            void Serialise(Road road)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", road.Id);
                writer.WriteNumber("length", road.Length.ToMetres());
                writer.WriteNumber("maxSpeed", road.MaxSpeed.ToKilometresPerHour());
                writer.WriteNumber("laneCount", road.LaneCount);
                writer.WriteEndObject();
            }

            writer.WriteStartObject();

            Road road = gRoad.GetRoad(IGRoad.Direction.Forward);
            if (road != null)
            {
                writer.WritePropertyName("forward");
                Serialise(road);
            }
            road = gRoad.GetRoad(IGRoad.Direction.Backward);
            if (road != null)
            {
                writer.WritePropertyName("backward");
                Serialise(road);
            }
            writer.WriteStartArray("route");
            foreach (var coords in gRoad.GetRoute())
                SerialiseCoords(writer, coords);
            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        private static void SerialiseCrossroad(Utf8JsonWriter writer, IGCrossroad gCrossroad, Crossroad crossroad)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("id");
            SerialiseCoords(writer, crossroad.Id);
            if (gCrossroad.MainRoadDirections.HasValue)
            {
                writer.WriteStartArray("mainRoadDirections");
                SerialiseCoords(writer, gCrossroad.MainRoadDirections.Value.Item1);
                SerialiseCoords(writer, gCrossroad.MainRoadDirections.Value.Item2);
                writer.WriteEndArray();
            }
            writer.WriteNumber("carSpawnRate", crossroad.CarSpawnRate);

            writer.WriteStartArray("trafficLightSettings");
            foreach (var setting in crossroad.TrafficLight.Settings)
            {
                writer.WriteStartObject();
                writer.WriteNumber("duration", setting.Duration.ToSeconds());
                writer.WriteStartArray("allowedDirections");
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
            writer.WriteNumber("x", coords.x);
            writer.WriteNumber("y", coords.y);
            writer.WriteEndObject();
        }

        private static void SerialiseDirection(Utf8JsonWriter writer, Direction direction)
        {
            writer.WriteStartObject();
            writer.WriteNumber("from", direction.fromRoadId);
            writer.WriteNumber("to", direction.toRoadId);
            writer.WriteEndObject();
        }

        private static string RoadToString(RoadView road)
        {
            StringBuilder sb = new StringBuilder();
            //foreach (int roadId in road.GuiRoad.GetRoads())
            //    sb.Append(roadId).Append(fieldSeparator);
            sb.Append(road.MaxSpeed).Append(fieldSeparator);
            foreach (Coords coords in road.GuiRoad.GetRoute())
                sb.Append(coords.x).Append(valueSeparator).Append(coords.y).Append(entitySeparator);
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static string CrossroadToString(CrossroadView crossroad)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(crossroad.Coords.x).Append(valueSeparator).Append(crossroad.Coords.y);
            sb.Append(fieldSeparator);
            sb.Append(crossroad.CarSpawnRate);
            sb.Append(fieldSeparator);
            foreach (TrafficLight.Setting setting in crossroad.TrafficLight.Settings)
            {
                sb.Append(setting.Duration).Append(entitySeparator);
                //foreach (TrafficLight.Direction direction in setting)
                //    sb.Append(direction.fromId).Append(valueSeparator).Append(direction.toId).Append(entitySeparator);
                sb.Remove(sb.Length - 1, 1);
                sb.Append(fieldSeparator);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static bool ParseRoad(string line, Map map, IGMap guiMap, out (int, int)[] roadIdMappings)
        {
            // Split line into fields and parse them: roadId [| backRoadId] | maxSpeed | path
            string[] fields = line.Split(fieldSeparator);
            if (fields.Length < 3 || fields.Length > 4)
            {
                roadIdMappings = null;
                return false;
            }
            roadIdMappings = new (int, int)[fields.Length - 2];
            if (!int.TryParse(fields[0], out roadIdMappings[0].Item1))
                return false;
            if (fields.Length == 4)
                if (!int.TryParse(fields[1], out roadIdMappings[1].Item1))
                    return false;
            if (!int.TryParse(fields[fields.Length - 2], out int maxSpeed))
                return false;
            // Split path into coords and get first two coords: coords1 ; coords2 [; coords3 [; coords4] ...]
            string[] path = fields[fields.Length - 1].Split(entitySeparator);
            if (path.Length < 2)
                return false;
            if (!ParseCoords(path[0], out Coords firstCoords))
                return false;
            if (!ParseCoords(path[1], out Coords secondCoords))
                return false;
            IRoadBuilder builder = MapManager.CreateRoadBuilder(map, guiMap, firstCoords, roadIdMappings.Length == 2);
            if (builder == null)
                return false;
            if (!builder.AddSegment(secondCoords))
            {
                builder.DestroyRoad();
                return false;
            }
            // Give the rest of the coords to the road builder
            for (int i = 2; i < path.Length; i++)
            {
                if (!ParseCoords(path[i], out Coords coords)
                    || !builder.AddSegment(coords))
                {
                    builder.DestroyRoad();
                    return false;
                }
            }
            // Build the road and get new road IDs
            if (!builder.FinishRoad(maxSpeed.MetresPerSecond()))
            {
                builder.DestroyRoad();
                return false;
            }
            IGRoad gRoad = guiMap.GetRoad(new Vector(firstCoords, secondCoords));
            int j = 0;
            foreach (var road in gRoad.GetRoads())
                roadIdMappings[j++].Item2 = road.Id;
            return true;
        }

        private static bool ParseCrossroad(string line, Components.Map map, Dictionary<int, int> roadIdMapper)
        {
            // Split line into fields and get the traffic light: coords | carSpawnRate [| settings1 [| settings2] ...]
            string[] fields = line.Split(fieldSeparator);
            if (fields.Length < 2)
                return false;
            if (!ParseCoords(fields[0], out Coords coords))
                return false;
            if (!byte.TryParse(fields[1], out byte carSpawnRate))
                return false;
            Components.Crossroad crossroad = (Components.Crossroad)map.GetNode(coords);
            TrafficLight trafficLight = crossroad.TrafficLight;
            crossroad.CarSpawnRate = carSpawnRate;
            // Split directions into roadID pairs and get the traffic light
            const int settingsOffset = 2;
            for (int i = settingsOffset; i < fields.Length; i++)
            {
                TrafficLight.Setting setting;
                if (i == settingsOffset)
                    setting = trafficLight.Settings[0];
                else
                    setting = trafficLight.InsertSetting(i - settingsOffset);
                // Add directions to the current setting: duration ; direction1 [; direction2 [; direction3] ...]
                string[] directions = fields[i].Split(entitySeparator);
                if (!int.TryParse(directions[0], out int duration))
                    return false;
                setting.Duration = duration.Seconds();
                for (int j = 1; j < directions.Length; j++)
                {
                    string[] split = directions[j].Split(valueSeparator);
                    if (split.Length != 2)
                        return false;
                    if (!int.TryParse(split[0], out int fromIdOld) || !int.TryParse(split[1], out int toIdOld))
                        return false;
                    if (!roadIdMapper.TryGetValue(fromIdOld, out int fromId) || !roadIdMapper.TryGetValue(toIdOld, out int toId))
                        return false;
                    if (crossroad.GetInEdge(fromId) == null || crossroad.GetOutEdge(toId) == null)
                        return false;
                    setting.AddDirection(fromId, toId);
                }
            }
            return true;
        }

        private static bool ParseCoords(string sCoords, out Coords coords)
        {
            coords = new Coords();
            string[] split = sCoords.Split(valueSeparator);
            if (split.Length != 2)
                return false;
            if (!int.TryParse(split[0], out int x) || !int.TryParse(split[1], out int y))
                return false;
            coords = new Coords(x, y);
            return true;
        }

        private static bool IgnoreLine(string line)
        {
            return line == string.Empty || line[0] == commentMark;
        }
    }
}
