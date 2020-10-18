using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.GUI;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    static class MapSaverLoader
    {
        private const string keywordRoads = "===ROADS===";
        private const string keywordTrafficLights = "===TRAFFIC_LIGHTS===";
        private const string keywordEnd = "===END===";
        private const char fieldSeparator = '|';
        private const char entitySeparator = ';';
        private const char valueSeparator = ',';

        /**
         * Format:
         * (without any spaces, they are added just for visual clarity)
         * 
         * ===ROADS===
         * roadId [| backRoadId] | maxSpeed | path
         * ...
         * ===TRAFFIC_LIGHTS===
         * coords [| settings1 [| settings2] ...]
         * ...
         * ===END===
         * 
         * where
         * path: coords1 ; coords2 [; coords3 [; coords4] ...]
         * coords: x,y
         * settings: direction1 [; direction2 [; direction3] ...]
         * direction: from,to
         */

        public static void SaveMap(StreamWriter writer, Components.Map map, IMap guiMap)
        {
            writer.WriteLine(keywordRoads);
            foreach (IRoad guiRoad in guiMap.GetRoads())
                writer.WriteLine(RoadToString(new RoadView((Components.Road)map.GetEdge(guiRoad.GetRoadIds().First()), guiRoad)));
            writer.WriteLine(keywordTrafficLights);
            foreach (ICrossroad guiCrossroad in guiMap.GetCrossroads())
                writer.WriteLine(CrossroadToString(new CrossroadView((Components.Crossroad)map.GetNode(guiCrossroad.CrossroadId), guiCrossroad)));
            writer.WriteLine(keywordEnd);
        }

        public static bool LoadMap(StreamReader reader, Components.Map map, IMap guiMap)
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
                else if (line == keywordTrafficLights)
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

        private static string RoadToString(RoadView road)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int roadId in road.GuiRoad.GetRoadIds())
                sb.Append(roadId).Append(fieldSeparator);
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
            foreach (TrafficLight.Setting setting in crossroad.TrafficLight.Settings)
            {
                foreach (TrafficLight.Direction direction in setting)
                    sb.Append(direction.fromId).Append(valueSeparator).Append(direction.toId).Append(entitySeparator);
                sb.Remove(sb.Length - 1, 1);
                sb.Append(fieldSeparator);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        private static bool ParseRoad(string line, Components.Map map, IMap guiMap, out (int, int)[] roadIdMappings)
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
            if (!builder.FinishRoad(maxSpeed.MetersPerSecond()))
            {
                builder.DestroyRoad();
                return false;
            }
            IRoad road = guiMap.GetRoad(new Vector(firstCoords, secondCoords), true);
            int j = 0;
            foreach (int id in road.GetRoadIds())
                roadIdMappings[j++].Item2 = id;
            return true;
        }

        private static bool ParseCrossroad(string line, Components.Map map, Dictionary<int, int> roadIdMapper)
        {
            // Split line into fields and get the traffic light: coords [| settings1 [| settings2] ...]
            string[] fields = line.Split(fieldSeparator);
            if (fields.Length < 1)
                return false;
            if (!ParseCoords(fields[0], out Coords coords))
                return false;
            Components.Crossroad crossroad = (Components.Crossroad)map.GetNode(coords);
            TrafficLight trafficLight = crossroad.TrafficLight;
            // Split directions into roadID pairs and get the traffic light
            for (int i = 1; i < fields.Length; i++)
            {
                TrafficLight.Setting setting;
                if (i == 1)
                    setting = trafficLight.Settings[0];
                else
                    setting = trafficLight.InsertSetting(i - 1);
                // Add directions to the current setting: direction1 [; direction2 [; direction3] ...]
                string[] directions = fields[i].Split(entitySeparator);
                foreach (string pair in directions)
                {
                    string[] split = pair.Split(valueSeparator);
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
            return line == string.Empty || line[0] == '#';
        }
    }
}
