using System;
using System.Collections;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents traffic lights at a crossroad.
    /// </summary>
    class TrafficLight : ICrossingAlgorithm
    {
        /// <summary>
        /// The maximum allowed number of traffic light settings (phases) for one traffic light
        /// </summary>
        public const int maxSettingsCount = 6;
        public const int minSettingsCount = 2;

        private static readonly Time yellowLightDuration = 3.Seconds();

        private HashSet<Direction> defaultDirections = new(4);
        private List<Setting> settings;
        private IEnumerator<Setting> settingsEnumerator;
        private Time currentTime;

        private Setting CurrentSetting { get => settingsEnumerator.Current; }
        private bool CurrentSettingExpired { get => currentTime >= CurrentSetting.Duration; }

        /// <summary>
        /// List of all settings cycled through by the traffic light
        /// </summary>
        public IReadOnlyList<Setting> Settings { get => settings; }

        /// <summary>
        /// Creates a new traffic light with one setting.
        /// </summary>
        public TrafficLight()
        {
            settings = new List<Setting>(maxSettingsCount);
            for (int i = 0; i < minSettingsCount; i++)
                settings.Add(new Setting());
        }

        #region methods

        /// <summary>
        /// Adds a direction that is allowed by default for all current and future settings on the traffic light.
        /// </summary>
        /// <remarks>
        /// Default directions can be used for two-way roads to always allow a 180-degrees turnaround.
        /// </remarks>
        /// <param name="fromId">ID of an ingoing road</param>
        /// <param name="toId">ID of an outgoing road</param>
        public void AddDefaultDirection(int fromId, int toId)
        {
            Direction direction = new(fromId, toId);
            defaultDirections.Add(direction);
            foreach (Setting s in settings)
                s.AddDirection(direction);
        }

        /// <summary>
        /// Adds a new setting to the traffic light.
        /// If <see cref="maxSettingsCount"/> has been reached, no setting is added.
        /// </summary>
        /// <returns>The new setting if successfully added, otherwise <c>null</c></returns>
        public Setting AddSetting()
        {
            if (settings.Count >= maxSettingsCount)
                return null;
            Setting output = new(defaultDirections);
            settings.Add(output);
            return output;
        }

        /// <summary>
        /// Inserts a new setting at a given index to the traffic light.
        /// If the index is invalid or <see cref="maxSettingsCount"/> has been reached, no setting is added.
        /// </summary>
        /// <returns>The new setting if successfully added, otherwise <c>null</c></returns>
        public Setting InsertSetting(int index)
        {
            if (settings.Count >= maxSettingsCount || index > settings.Count)
                return null;
            Setting output = new(defaultDirections);
            settings.Insert(index, output);
            return output;
        }

        /// <summary>
        /// Removes a setting at a given index.
        /// </summary>
        /// <returns><c>true</c> if successfully removed, otherwise <c>false</c></returns>
        public bool RemoveSetting(int index)
        {
            if (settings.Count <= minSettingsCount || index >= settings.Count)
                return false;
            settings.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes a given road from all settings and default directions.
        /// </summary>
        /// <param name="id">ID of the removed road</param>
        public void RemoveRoad(int id)
        {
            foreach (Setting setting in Settings)
                setting.RemoveDirectionsWithId(id);
            defaultDirections.RemoveWhere(dir => id == dir.fromRoadId || id == dir.toRoadId);
        }

        /// <summary>
        /// Initialises the traffic light before starting a simulation.
        /// Checks whether all directions in a given verifier are allowed in at least one setting.
        /// If the consistency check is successful, sets the first setting as active.
        /// </summary>
        /// <param name="verifier">Dictionary containing checked directions as keys with all values <c>false</c></param>
        /// <returns><c>true</c> if the consistency check was successful, otherwise <c>false</c></returns>
        public bool Initialise(Dictionary<Direction, bool> verifier)
        {
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
            if (currentTime > CurrentSetting.Duration)
            {
                currentTime -= CurrentSetting.Duration;
                settingsEnumerator.MoveNext();
            }
            currentTime += time;
        }

        public bool CanCross(Car car, int fromRoadId, int toRoadId, Time expectedArrival)
        {
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

        /// <summary>
        /// Gets a cyclic (infinite) enumerator through all settings.
        /// </summary>
        private IEnumerator<Setting> GetSettingsEnumerator()
        {
            while (settings.Count > 0)
                foreach (Setting s in settings)
                    yield return s;
        }

        #endregion methods

        /// <summary>
        /// Represents a single setting of a traffic light.
        /// </summary>
        public class Setting : IReadOnlyCollection<Direction>
        {
            private static readonly Time minDuration = 10.Seconds();
            private static readonly Time maxDuration = 120.Seconds();

            private Time duration = 20.Seconds();
            private HashSet<Direction> allowedDirections = new(4);

            /// <summary>
            /// Duration of the setting before switching to the next one
            /// </summary>
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

            /// <summary>
            /// Number of allowed directions in the setting
            /// </summary>
            public int Count { get => allowedDirections.Count; }

            /// <summary>
            /// Creates a new empty setting.
            /// </summary>
            public Setting() { }

            /// <summary>
            /// Creates a new setting with a given set of directions allowed by default.
            /// </summary>
            public Setting(ICollection<Direction> defaultDirections)
            {
                foreach (Direction d in defaultDirections)
                    allowedDirections.Add(d);
            }

            /// <summary>
            /// Adds a new allowed direction to the setting.
            /// </summary>
            public void AddDirection(Direction direction)
            {
                allowedDirections.Add(direction);
            }

            /// <summary>
            /// Adds a new allowed direction to the setting between two given nodes.
            /// </summary>
            public void AddDirection(int fromId, int toId)
            {
                allowedDirections.Add(new Direction(fromId, toId));
            }

            /// <summary>
            /// Removes an allowed direction between two given nodes.
            /// </summary>
            public void RemoveDirection(int fromId, int toId)
            {
                allowedDirections.Remove(new Direction(fromId, toId));
            }

            /// <summary>
            /// Removes all allowed directions containing a given road ID.
            /// </summary>
            public void RemoveDirectionsWithId(int id)
            {
                allowedDirections.RemoveWhere(dir => id == dir.fromRoadId || id == dir.toRoadId);
            }

            /// <summary>
            /// Checks whether a direction between two given nodes is allowed by the setting.
            /// </summary>
            /// <returns><c>true</c> if the direction is allowed, otherwise <c>false</c></returns>
            public bool ContainsDirection(int fromId, int toId)
            {
                return allowedDirections.Contains(new Direction(fromId, toId));
            }

            /// <summary>
            /// Returns an enumerator that iterates through all directions allowed by the setting.
            /// </summary>
            public IEnumerator<Direction> GetEnumerator() => allowedDirections.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
