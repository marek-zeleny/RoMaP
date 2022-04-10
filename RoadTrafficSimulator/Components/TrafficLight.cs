using System;
using System.Collections;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Components
{
    class TrafficLight : ICrossingAlgorithm
    {
        public const int maxSettingsCount = 5;

        private static readonly Time yellowLightDuration = 3.Seconds(); // TODO: mention in thesis

        private HashSet<Direction> defaultDirections = new(4);
        private List<Setting> settings;
        private IEnumerator<Setting> settingsEnumerator;
        private Time currentTime;

        private Setting CurrentSetting { get => settingsEnumerator.Current; }
        private bool CurrentSettingExpired { get => currentTime >= CurrentSetting.Duration; }

        public IReadOnlyList<Setting> Settings { get => settings; }

        public TrafficLight()
        {
            settings = new List<Setting>(maxSettingsCount);
            settings.Add(new Setting());
        }

        #region methods

        public void AddDefaultDirection(int fromId, int toId)
        {
            Direction direction = new(fromId, toId);
            defaultDirections.Add(direction);
            foreach (Setting s in settings)
                s.AddDirection(direction);
        }

        public Setting AddSetting()
        {
            if (settings.Count >= maxSettingsCount)
                return null;
            Setting output = new(defaultDirections);
            settings.Add(output);
            return output;
        }

        public Setting InsertSetting(int index)
        {
            if (settings.Count >= maxSettingsCount || index > settings.Count)
                return null;
            Setting output = new Setting(defaultDirections);
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

        public void RemoveEdge(int id)
        {
            foreach (Setting setting in Settings)
                setting.RemoveDirectionsWithId(id);
            defaultDirections.RemoveWhere(dir => id == dir.fromRoadId || id == dir.toRoadId);
        }

        public bool Initialize(Dictionary<Direction, bool> verifier)
        {
            if (settings.Count <= 1)
                return true;

            foreach (Setting s in settings)
                foreach (Direction d in s)
                    verifier[d] = true;
            foreach (var (_, b) in verifier)
                if (!b)
                    return false;
            settingsEnumerator = GetSettingsEnumerator();
            return settingsEnumerator.MoveNext();
        }

        public void Tick(Time time)
        {
            if (settings.Count <= 1)
                return;
            if (currentTime > CurrentSetting.Duration)
            {
                currentTime -= CurrentSetting.Duration;
                settingsEnumerator.MoveNext();
            }
            currentTime += time;
        }

        public bool CanCross(Car car, int fromRoadId, int toRoadId, Time expectedArrival)
        {
            if (settings.Count <= 1)
                return false;
            if (CurrentSettingExpired)
                return false;
            if (CurrentSetting.ContainsDirection(fromRoadId, toRoadId))
            {
                // Check for yellow light
                Time remaining = CurrentSetting.Duration - currentTime;
                return remaining > yellowLightDuration || remaining < expectedArrival;
            }
            return false;
        }

        public void CarCrossed(Car car, int fromRoadId, int toRoadId) { }

        private IEnumerator<Setting> GetSettingsEnumerator()
        {
            while (settings.Count > 0)
                foreach (Setting s in settings)
                    yield return s;
        }

        #endregion methods

        public class Setting : IReadOnlyCollection<Direction>
        {
            private static readonly Time minDuration = 5.Seconds();
            private static readonly Time maxDuration = 120.Seconds();

            private Time duration = 20.Seconds();
            private HashSet<Direction> allowedDirections = new(4);

            public Time Duration
            {
                get => duration;
                set
                {
                    if (value < minDuration)
                        duration = minDuration;
                    else if (value > maxDuration)
                        duration = maxDuration;
                    else
                        duration = value;
                }
            }

            public int Count { get => allowedDirections.Count; }

            public Setting() { }

            public Setting(ICollection<Direction> defaultDirections)
            {
                foreach (Direction d in defaultDirections)
                    allowedDirections.Add(d);
            }

            public void AddDirection(Direction direction)
            {
                allowedDirections.Add(direction);
            }

            public void AddDirection(int fromId, int toId)
            {
                allowedDirections.Add(new Direction(fromId, toId));
            }

            public void RemoveDirection(int fromId, int toId)
            {
                allowedDirections.Remove(new Direction(fromId, toId));
            }

            public void RemoveDirectionsWithId(int id)
            {
                allowedDirections.RemoveWhere(dir => id == dir.fromRoadId || id == dir.toRoadId);
            }

            public bool ContainsDirection(int fromId, int toId)
            {
                return allowedDirections.Contains(new Direction(fromId, toId));
            }

            public IEnumerator<Direction> GetEnumerator() => allowedDirections.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
