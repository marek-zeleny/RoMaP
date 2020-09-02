using System;
using System.Collections;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components
{
    class TrafficLight
    {
        private const int maxSettingsCount = 5;

        private List<Setting> settings = new List<Setting>(maxSettingsCount);
        private IEnumerator<Setting> settingEnumerator;
        private Seconds currentTime;

        private bool CurrentSettingExpired { get => currentTime >= settingEnumerator.Current.Duration; }

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

        public bool Initialize(Dictionary<Direction, bool> verifier)
        {
            foreach (Setting s in settings)
                foreach (Direction d in s)
                    verifier[d] = true;
            foreach (var (_, b) in verifier)
                if (!b)
                    return false;
            settingEnumerator = GetEnumerator();
            return true;
        }

        public void Tick(Seconds time)
        {
            if (currentTime > settingEnumerator.Current.Duration)
            {
                currentTime -= settingEnumerator.Current.Duration;
                settingEnumerator.MoveNext();
            }
            currentTime += time;
        }

        public bool DirectionAllowed(int fromId, int toId)
        {
            return !CurrentSettingExpired && settingEnumerator.Current.ContainsDirection(fromId, toId);
        }

        private IEnumerator<Setting> GetEnumerator()
        {
            while (settings.Count > 0)
                foreach (Setting s in settings)
                    yield return s;
        }

        public struct Setting : IReadOnlyList<Direction>
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
        }

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
