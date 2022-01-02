using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

using DataStructures.Graphs;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    class Road : Edge<Coords, int>
    {
        public static readonly Distance minLength = 10.Metres();
        public static readonly Speed minMaxSpeed = 3.MetresPerSecond();
        public const int maxLaneCount = 3;
        private const int averageDurationHistorySize = 10;

        private Distance length;
        private Speed maxSpeed;
        private Lane[] lanes;
        private int laneCount;
        private Queue<Time> averageDurationHistory;
        private RoadStatistics statistics;

        public Distance Length
        {
            get => length;
            set
            {
                if (value < minLength)
                    length = minLength;
                else
                    length = value;
                Weight = (Length / MaxSpeed).Weight();
            }
        }
        public Speed MaxSpeed
        {
            get => maxSpeed;
            set
            {
                if (value < minMaxSpeed)
                    maxSpeed = minMaxSpeed;
                else
                    maxSpeed = value;
                Weight = (Length / MaxSpeed).Weight();
            }
        }
        public Time AverageDuration { get; private set; }
        public Speed AverageSpeed { get; private set; }
        public int CarCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < LaneCount; i++)
                    count += lanes[i].CarCount;
                return count;
            }
        }
        public int LaneCount
        {
            get => laneCount;
            set
            {
                int newLaneCount = value;
                if (newLaneCount < 1)
                    newLaneCount = 1;
                else if (newLaneCount > maxLaneCount)
                    newLaneCount = maxLaneCount;
                if (newLaneCount < laneCount)
                    for (int i = newLaneCount; i < laneCount; i++)
                        lanes[i].Initialise();
                laneCount = newLaneCount;
            }
        }
        public Crossroad Destination { get => (Crossroad)ToNode; }
        public IRoadStatistics Statistics { get => statistics; }

        public Road(int id, Crossroad from, Crossroad to, Distance length, Speed maxSpeed)
            : base(id, from, to)
        {
            if (length < minLength)
                length = minLength;
            if (maxSpeed < minMaxSpeed)
                maxSpeed = minMaxSpeed;
            this.length = length;
            this.maxSpeed = maxSpeed;
            Weight = (length / maxSpeed).Weight();
            lanes = new Lane[maxLaneCount];
            LaneCount = 1;
            lanes[0].Initialise();
        }

        #region methods

        public override void SetWeight(Weight value)
        {
            throw new InvalidOperationException($"Cannot explicitly set weight of a {nameof(Road)}.");
        }

        public bool Initialise(StatisticsCollector collector, IClock clock)
        {
            for (int i = 0; i < LaneCount; i++)
                lanes[i].Initialise();
            AverageDuration = Length / MaxSpeed;
            AverageSpeed = MaxSpeed;
            averageDurationHistory = new Queue<Time>(averageDurationHistorySize);
            statistics = new RoadStatistics(collector, clock, Id);
            return true;
        }

        public bool TryGetOn(Car car, out Car carInFront)
        {
            int maxIndex = 0;
            Distance maxSpace = lanes[0].FreeSpace(Length);
            for (int i = 1; i < LaneCount; i++)
            {
                Distance space = lanes[i].FreeSpace(Length);
                if (space > maxSpace)
                {
                    maxIndex = i;
                    maxSpace = space;
                }
            }
            return lanes[maxIndex].TryGetOn(this, car, out carInFront);
        }

        public void GetOff(Car car)
        {
            for (int i = 0; i < LaneCount; i++)
            {
                if (lanes[i].TryGetOff(this, car, out Time arriveTime))
                {
                    Time duration = statistics.CarGotOff(car.Id, arriveTime);
                    if (averageDurationHistory.Count >= averageDurationHistorySize)
                        averageDurationHistory.Dequeue();
                    averageDurationHistory.Enqueue(duration);
                    Time totalDuration = averageDurationHistory.Aggregate((acc, dur) => acc + dur);
                    AverageDuration = totalDuration / averageDurationHistory.Count;
                    return;
                }
            }
            throw new ArgumentException("The car must be first in a lane to get off the road.", nameof(car));
        }

        public void Tick(Time time)
        {
            // We don't want to include into statistics cars that got off the road (it could mess up average speed)
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car => car.Tick(time));
            // Cars that haven't yet gotten onto the road will be missed in this round of statistics, but that's a small
            // error
            int carCount = 0;
            Speed totalSpeed = 0.MetresPerSecond();
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car =>
                {
                    carCount++;
                    totalSpeed += car.CurrentSpeed;
                });
            if (carCount == 0)
                AverageSpeed = MaxSpeed;
            else
                AverageSpeed = totalSpeed / carCount;
            statistics.Update(CarCount, AverageSpeed, AverageDuration);
        }

        public void AfterTick()
        {
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car => car.AfterTick());
        }

        #endregion methods

        #region subclasses

        private struct Lane
        {
            private Car firstCar;
            private Car lastCar;
            private Queue<Time> arriveTimes;

            public int CarCount { get => arriveTimes.Count; }

            public void Initialise()
            {
                firstCar = null;
                lastCar = null;
                arriveTimes = new Queue<Time>();
            }

            public Distance FreeSpace(Distance length)
            {
                return lastCar == null ? length : lastCar.DistanceRear;
            }

            public bool TryGetOn(Road road, Car car, out Car carInFront)
            {
                carInFront = lastCar;
                if (firstCar == null)
                    firstCar = car;
                else if (lastCar.DistanceRear < car.Length)
                    return false;
                else
                    lastCar.SetCarBehind(road, car);
                lastCar = car;
                Time time = road.statistics.CarGotOn(car.Id);
                arriveTimes.Enqueue(time);
                return true;
            }

            public bool TryGetOff(Road road, Car car, out Time arriveTime)
            {
                if (car != firstCar)
                {
                    arriveTime = default;
                    return false;
                }
                firstCar = firstCar.CarBehind;
                if (firstCar == null)
                    lastCar = null;
                else
                    firstCar.RemoveCarInFront(road);
                arriveTime = arriveTimes.Dequeue();
                return true;
            }

            public void ForAllCars(Action<Car> action)
            {
                Car current = firstCar;
                // Because the current Car can leave the Road during its action, we need to know the next Car beforehand
                Car next;
                while (current != null)
                {
                    next = current.CarBehind;
                    action(current);
                    current = next;
                }
            }
        }

        public interface IRoadStatistics
        {
            public int RoadId { get; }
            public IReadOnlyList<Timestamp<CarPassage>> CarLog { get; }
            public IReadOnlyList<Timestamp<Throughput>> ThroughputLog { get; }
        }

        public readonly struct CarPassage
        {
            public readonly int carId;
            public readonly Time arriveTime;

            public CarPassage(int carId, Time arriveTime)
            {
                this.carId = carId;
                this.arriveTime = arriveTime;
            }
        }

        public readonly struct Throughput
        {
            public readonly int carCount;
            public readonly Speed averageSpeed;
            public readonly Time averageDuration;

            public Throughput(int carCount, Speed averageSpeed, Time averageDuration)
            {
                this.carCount = carCount;
                this.averageSpeed = averageSpeed;
                this.averageDuration = averageDuration;
            }
        }

        private class RoadStatistics : StatisticsBase, IRoadStatistics
        {
            Item<List<Timestamp<CarPassage>>> carLog = new(DetailLevel.Medium, new());
            Item<List<Timestamp<Throughput>>> throughputLog = new(DetailLevel.High, new());

            public int RoadId { get; }
            public IReadOnlyList<Timestamp<CarPassage>> CarLog { get => carLog.Get(); }
            public IReadOnlyList<Timestamp<Throughput>> ThroughputLog { get => throughputLog.Get(); }

            public RoadStatistics(StatisticsCollector collector, IClock clock, int roadId)
                : base(collector, typeof(Road), clock)
            {
                RoadId = roadId;
            }

            public void Update(int carCount, Speed averageSpeed, Time averageDuration)
            {
                throughputLog.Get()?.Add(new Timestamp<Throughput>(
                    clock.Time, new Throughput(carCount, averageSpeed, averageDuration)));
            }

            public Time CarGotOn(int carId)
            {
                return clock.Time;
            }

            public Time CarGotOff(int carId, Time arriveTime)
            {
                carLog.Get()?.Add(new Timestamp<CarPassage>(clock.Time, new CarPassage(carId, arriveTime)));
                return clock.Time - arriveTime;
            }

            public override void Serialise(Utf8JsonWriter writer)
            {
                static void SerialiseCarPassage(Utf8JsonWriter writer, CarPassage passage)
                {
                    writer.WriteNumber("arriveTime", passage.arriveTime);
                    writer.WriteNumber("carId", passage.carId);
                }
                static void SerialiseThroughput(Utf8JsonWriter writer, Throughput throughput)
                {
                    writer.WriteNumber("carCount", throughput.carCount);
                    writer.WriteNumber("averageSpeed", throughput.averageSpeed);
                    writer.WriteNumber("averageDuration", throughput.averageDuration);
                }

                writer.WriteStartObject();
                writer.WriteNumber("id", RoadId);

                SerialiseTimestampListItem(writer, carLog, "carLog", SerialiseCarPassage);
                SerialiseTimestampListItem(writer, throughputLog, "throughputLog", SerialiseThroughput);

                writer.WriteEndObject();
            }
        }

        #endregion subclasses
    }
}
