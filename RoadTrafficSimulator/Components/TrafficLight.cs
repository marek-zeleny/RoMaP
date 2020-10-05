using System;
using System.Collections;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Components
{
    class TrafficLight
    {
        private const int maxSettingsCount = 5;

        private List<Setting> settings;
        private IEnumerator<Setting> settingEnumerator;
        private Seconds currentTime;

        private bool CurrentSettingExpired { get => currentTime >= settingEnumerator.Current.Duration; }

        public IReadOnlyList<Setting> Settings { get => settings; }

        public TrafficLight()
        {
            settings = new List<Setting>(maxSettingsCount);
            settings.Add(new Setting());
        }

        public Setting InsertSetting(int index)
        {
            if (settings.Count >= maxSettingsCount || index > settings.Count)
                return null;
            Setting output = new Setting();
            settings.Insert(index, output);
            return output;
        }

        public bool RemoveSetting(int index)
        {
            if (index >= settings.Count || settings.Count <= 1)
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

        public class Setting : IReadOnlyList<Direction>
        {
            private List<Direction> allowedDirections = new List<Direction>();

            public Seconds Duration { get; set; } = 20.Seconds();

            public Direction this[int index] { get => allowedDirections[index]; }

            public int Count { get => allowedDirections.Count; }

            public void AddDirection(int fromId, int toId)
            {
                allowedDirections.Add(new Direction(fromId, toId));
            }

            public bool RemoveDirection(int index)
            {
                if (index >= allowedDirections.Count)
                    return false;
                allowedDirections.RemoveAt(index);
                return true;
            }

            public bool ContainsDirection(int fromId, int toId)
            {
                return allowedDirections.Contains(new Direction(fromId, toId));
            }

            public IEnumerator<Direction> GetEnumerator() => allowedDirections.GetEnumerator();

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
