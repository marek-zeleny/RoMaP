using System;
using System.Collections.Generic;
using System.Linq;

using RoadTrafficSimulator.Components;
using RoadTrafficSimulator.ValueTypes;

namespace RoadTrafficSimulator
{
    class Statistics
    {
        private List<Record> records = new List<Record>();

        public int RecordCount { get => records.Count; }

        public void AddRecord(Car car, Seconds currentTime)
        {
            records.Add(new Record(car.TotalDistance, car.ExpectedDuration, currentTime - car.StartTime));
        }

        public Meters GetAverageDistance()
        {
            if (RecordCount == 0)
                return 0.Meters();
            int totalDistance = records.Sum(record => record.traveledDistance);
            return (totalDistance / RecordCount).Meters();
        }

        public Seconds GetAverageDuration()
        {
            if (RecordCount == 0)
                return 0.Seconds();
            int totalDuration = records.Sum(record => record.actualDuration);
            return (totalDuration / RecordCount).Seconds();
        }

        public Seconds GetAverageDelay()
        {
            if (RecordCount == 0)
                return 0.Seconds();
            int totalDelay = records.Sum(record => record.actualDuration - record.expectedDuration);
            return (totalDelay / RecordCount).Seconds();
        }

        private struct Record
        {
            public readonly Meters traveledDistance;
            public readonly Seconds expectedDuration;
            public readonly Seconds actualDuration;

            public Record(Meters traveledDistance, Seconds expectedDuration, Seconds actualDuration)
            {
                this.traveledDistance = traveledDistance;
                this.expectedDuration = expectedDuration;
                this.actualDuration = actualDuration;
            }
        }
    }
}
