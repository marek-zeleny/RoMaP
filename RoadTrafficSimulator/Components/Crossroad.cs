using System;
using System.Collections;
using System.Collections.Generic;

using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Crossroad : Node<int, int>
    {
        private IEnumerator<TrafficLight.Setting> settingEnumerator;
        private Seconds currentTime;

        private TrafficLight.Setting CurrentSetting { get => settingEnumerator.Current; }
        private bool AllowMovement { get => currentTime < CurrentSetting.Duration; }

        public TrafficLight Light { get; }

        public Crossroad(int id)
            : base(id)
        {
            Light = new TrafficLight();
        }

        public bool Initialize()
        {
            settingEnumerator = Light.GetEnumerator();
            return settingEnumerator.MoveNext();
        }

        public void Tick(Seconds time)
        {
            if (currentTime > CurrentSetting.Duration)
            {
                currentTime -= CurrentSetting.Duration;
                settingEnumerator.MoveNext();
            }
            currentTime += time;
        }

        public bool CrossingAllowed(int fromRoadId, int toRoadId)
        {
            return AllowMovement && CurrentSetting.ContainsDirection(fromRoadId, toRoadId);
        }

        public class TrafficLight : IEnumerable<TrafficLight.Setting>
        {
            private const int maxSettingsCount = 5;

            private List<Setting> settings = new List<Setting>(maxSettingsCount);

            public IReadOnlyList<Setting> GetSettings() => settings;

            public Setting? InsertSetting(int index)
            {
                if (settings.Count >= maxSettingsCount)
                    return null;
                Setting output = new Setting();
                settings.Insert(index, output);
                return output;
            }

            public bool RemoveSetting(int index)
            {
                if (index >= settings.Count)
                    return false;
                settings.RemoveAt(index);
                return true;
            }

            public IEnumerator<Setting> GetEnumerator()
            {
                while (settings.Count > 0)
                    foreach (Setting s in settings)
                        yield return s;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public struct Setting : IReadOnlyList<Setting.Direction>
            {
                private List<Direction> allowedDirections;

                public Seconds Duration { get; set; }

                private List<Direction> AllowedDirections
                {
                    get
                    {
                        if (allowedDirections == null)
                            allowedDirections = new List<Direction>();
                        return allowedDirections;
                    }
                }

                public Direction this[int index] { get => AllowedDirections[index]; }

                public int Count { get => AllowedDirections.Count; }

                public void AddDirection(int fromId, int toId)
                {
                    AllowedDirections.Add(new Direction(fromId, toId));
                }

                public bool RemoveDirection(int index)
                {
                    if (index >= AllowedDirections.Count)
                        return false;
                    AllowedDirections.RemoveAt(index);
                    return true;
                }

                public bool ContainsDirection(int fromId, int toId)
                {
                    return AllowedDirections.Contains(new Direction(fromId, toId));
                }

                public IEnumerator<Direction> GetEnumerator() => AllowedDirections.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                public struct Direction
                {
                    public readonly int fromId;
                    public readonly int toId;

                    public Direction(int fromId, int toId)
                    {
                        this.fromId = fromId;
                        this.toId = toId;
                    }
                }
            }
        }
    }
}
