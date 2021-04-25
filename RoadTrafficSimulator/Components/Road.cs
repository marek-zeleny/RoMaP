using System;
using System.Linq;
using System.Collections.Generic;

using DataStructures.Graphs;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    class Road : Edge<Coords, int>
    {
        public static readonly Millimetres minLength = 10.Metres();
        public static readonly MetresPerSecond minMaxSpeed = 3.MetresPerSecond();
        public const int maxLaneCount = 3;
        private const int averageDurationHistorySize = 10;

        private Millimetres length;
        private MetresPerSecond maxSpeed;
        private Lane[] lanes;
        private int laneCount;
        private Queue<Milliseconds> averageDurationHistory;
        private Statistics statistics;

        public Millimetres Length
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
        public MetresPerSecond MaxSpeed
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
        public Milliseconds AverageDuration { get; private set; }
        public MetresPerSecond AverageSpeed { get; private set; }
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

        public Road(int id, Crossroad from, Crossroad to, Millimetres length, MetresPerSecond maxSpeed)
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

        public bool Initialise(IClock clock)
        {
            for (int i = 0; i < LaneCount; i++)
                lanes[i].Initialise();
            AverageDuration = Length / MaxSpeed;
            AverageSpeed = MaxSpeed;
            averageDurationHistory = new Queue<Milliseconds>(averageDurationHistorySize);
            statistics = new Statistics(clock, Id);
            return true;
        }

        public bool TryGetOn(Car car, out Car carInFront)
        {
            int maxIndex = 0;
            Millimetres maxSpace = lanes[0].FreeSpace(Length);
            for (int i = 1; i < LaneCount; i++)
            {
                Millimetres space = lanes[i].FreeSpace(Length);
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
                if (lanes[i].TryGetOff(this, car, out Milliseconds arriveTime))
                {
                    Milliseconds duration = statistics.CarGotOff(car.Id, arriveTime);
                    if (averageDurationHistory.Count >= averageDurationHistorySize)
                        averageDurationHistory.Dequeue();
                    averageDurationHistory.Enqueue(duration);
                    Milliseconds totalDuration = averageDurationHistory.Aggregate((acc, dur) => acc + dur);
                    AverageDuration = totalDuration / averageDurationHistory.Count;
                    return;
                }
            }
            throw new ArgumentException("The car must be first in a lane to get off the road.", nameof(car));
        }

        public void Tick(Milliseconds time)
        {
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car => car.Tick(time));

            int carCount = 0;
            MetresPerSecond totalSpeed = 0.MetresPerSecond();
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car =>
                {
                    car.FinishCrossingRoads(time);
                    carCount++;
                    totalSpeed += car.CurrentSpeed;
                });
            if (carCount == 0)
                AverageSpeed = MaxSpeed;
            else
                AverageSpeed = totalSpeed / carCount;
            statistics.Update(CarCount, AverageSpeed, AverageDuration);
        }

        #endregion methods

        #region subclasses

        private struct Lane
        {
            private Car firstCar;
            private Car lastCar;
            private Queue<Milliseconds> arriveTimes;

            public int CarCount { get => arriveTimes.Count; }

            public void Initialise()
            {
                firstCar = null;
                lastCar = null;
                arriveTimes = new Queue<Milliseconds>();
            }

            public Millimetres FreeSpace(Millimetres length)
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
                Milliseconds time = road.statistics.CarGotOn(car.Id);
                arriveTimes.Enqueue(time);
                return true;
            }

            public bool TryGetOff(Road road, Car car, out Milliseconds arriveTime)
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

        public class Statistics : StatisticsBase
        {
            Item<List<Timestamp<CarPassage>>> carLog = new Item<List<Timestamp<CarPassage>>>(DetailLevel.Medium,
                new List<Timestamp<CarPassage>>());
            Item<List<Timestamp<Throughput>>> throughputLog = new Item<List<Timestamp<Throughput>>>(DetailLevel.High,
                new List<Timestamp<Throughput>>());

            public int RoadId { get; }
            public IReadOnlyList<Timestamp<CarPassage>> CarLog { get => carLog.Get(); }
            public IReadOnlyList<Timestamp<Throughput>> ThroughputLog { get => throughputLog.Get(); }

            public Statistics(IClock clock, int roadId)
                : base(clock)
            {
                RoadId = roadId;
            }

            public void Update(int carCount, MetresPerSecond averageSpeed, Milliseconds averageDuration)
            {
                throughputLog.Get()?.Add(new Timestamp<Throughput>(clock.Time,
                    new Throughput(carCount, averageSpeed, averageDuration)));
            }

            public Milliseconds CarGotOn(int carId)
            {
                return clock.Time;
            }

            public Milliseconds CarGotOff(int carId, Milliseconds arriveTime)
            {
                carLog.Get()?.Add(new Timestamp<CarPassage>(clock.Time, new CarPassage(carId, arriveTime)));
                return clock.Time - arriveTime;
            }

            public struct CarPassage
            {
                public readonly int carId;
                public readonly Milliseconds arriveTime;

                public CarPassage(int carId, Milliseconds arriveTime)
                {
                    this.carId = carId;
                    this.arriveTime = arriveTime;
                }
            }

            public struct Throughput
            {
                public int carCount;
                public readonly MetresPerSecond averageSpeed;
                public readonly Milliseconds averageDuration;

                public Throughput(int carCount, MetresPerSecond averageSpeed, Milliseconds averageDuration)
                {
                    this.carCount = carCount;
                    this.averageSpeed = averageSpeed;
                    this.averageDuration = averageDuration;
                }
            }
        }

        #endregion subclasses
    }
}
