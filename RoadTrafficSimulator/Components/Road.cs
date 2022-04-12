using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Diagnostics;

using DataStructures.Graphs;
using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents a road in a map.
    /// </summary>
    class Road : Edge<Coords, int>
    {
        public static readonly Distance minLength = 20.Metres();
        public static readonly Speed minMaxSpeed = 10.KilometresPerHour();
        public const int maxLaneCount = 3;
        private const int averageDurationHistorySize = 10;

        private Distance length;
        private Speed maxSpeed;
        private Lane[] lanes;
        private int laneCount;
        private Queue<Time> averageDurationHistory;
        private RoadStatistics statistics;

        /// <summary>
        /// Length of the road; must be at least <see cref="minLength"/>
        /// </summary>
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
                AverageDuration = Length / MaxSpeed;
            }
        }
        /// <summary>
        /// Maximum allowed speed on the road; must be at least <see cref="minMaxSpeed"/>
        /// </summary>
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
                AverageDuration = Length / MaxSpeed;
                AverageSpeed = MaxSpeed;
            }
        }
        /// <summary>
        /// Average duration of a car's travel through the road, computed as a mean value of the last few cars that
        /// reached the end
        /// </summary>
        public Time AverageDuration { get; private set; }
        /// <summary>
        /// Average speed of cars currently on the road
        /// </summary>
        public Speed AverageSpeed { get; private set; }
        /// <summary>
        /// Total number of cars currently on the road
        /// </summary>
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
        /// <summary>
        /// Number of lanes of the road; must be at most <see cref="maxLaneCount"/>
        /// </summary>
        /// <remarks>
        /// Cars cannot cross between lanes, but more lanes increase the road's throughput.
        /// </remarks>
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
        /// <summary>
        /// Crossroad at the end of the road
        /// </summary>
        public Crossroad Destination { get => (Crossroad)ToNode; }
        /// <summary>
        /// Statistics collected by the road during the simulation
        /// </summary>
        public IRoadStatistics Statistics { get => statistics; }
        /// <summary>
        /// <c>true</c> if the road is currently connected to a graph, otherwise <c>false</c>
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Creates a new road between two given crossroads with a given ID, length and maximal speed.
        /// </summary>
        public Road(int id, Crossroad from, Crossroad to, Distance length, Speed maxSpeed)
            : base(id, from, to)
        {
            Length = length;
            MaxSpeed = maxSpeed;
            Weight = (Length / MaxSpeed).Weight();
            lanes = new Lane[maxLaneCount];
            LaneCount = 1;
        }

        /// <summary>
        /// Creates a new road between two given crossroads copying the properties of an existing road.
        /// </summary>
        public Road(int id, Crossroad from, Crossroad to, Road originalRoad)
            : base(id, from, to)
        {
            Length = originalRoad.length;
            MaxSpeed = originalRoad.maxSpeed;
            Weight = originalRoad.Weight;
            lanes = originalRoad.lanes;
            laneCount = originalRoad.laneCount;
        }

        #region methods

        public override void SetWeight(Weight value)
        {
            throw new InvalidOperationException($"Cannot explicitly set weight of a {nameof(Road)}.");
        }

        /// <summary>
        /// Performs necessary initial actions and checks before starting a simulation.
        /// </summary>
        /// <param name="collector">Statistics collector necessary to collect the road's statistics</param>
        /// <param name="clock">Simulation clock necessary for the road's functioning</param>
        /// <returns><c>true</c> if all checks are successful, otherwise <c>false</c></returns>
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

        /// <summary>
        /// Tries to place a given car at the beginning of one of the lanes.
        /// </summary>
        /// <param name="carInFront">
        /// If successful, returns the car that was last in the lane until now (<c>null</c> if there was no car);
        /// undefined otherwise
        /// </param>
        /// <returns><c>true</c> the car was successfully placed, otherwise <c>false</c></returns>
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

        /// <summary>
        /// Removes a car from the beginning of some lane.
        /// </summary>
        /// <exception cref="ArgumentException">The given car was not the first in its lane.</exception>
        public void GetOff(Car car)
        {
            for (int i = 0; i < LaneCount; i++)
            {
                if (lanes[i].TryGetOff(this, car, out Time duration))
                {
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

        /// <summary>
        /// Performs a simulation step of given time duration on all cars on the road and updates statistics.
        /// </summary>
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
            Debug.Assert(AverageSpeed >= 0);
            statistics.Update(CarCount, AverageSpeed, AverageDuration);
        }

        #endregion methods

        #region subclasses

        /// <summary>
        /// Represents a lane on the road.
        /// </summary>
        private struct Lane
        {
            private Car firstCar;
            private Car lastCar;
            private Queue<Time> arriveTimes;

            /// <summary>
            /// Number of cars currently on the lane
            /// </summary>
            public int CarCount { get => arriveTimes.Count; }

            /// <summary>
            /// Initialises the lane before simulation start.
            /// </summary>
            public void Initialise()
            {
                firstCar = null;
                lastCar = null;
                arriveTimes = new Queue<Time>();
            }

            /// <summary>
            /// Calculates free space at the beginning of the lane before encountering the first car.
            /// </summary>
            /// <param name="length">Length of the lane (only known by the owning road)</param>
            /// <returns>Free space at the beginning; may be negative</returns>
            public Distance FreeSpace(Distance length)
            {
                return lastCar == null ? length : lastCar.DistanceRear - Car.minDistanceBetweenCars;
            }

            /// <summary>
            /// Tries to place a given car at the beginning of the lane. If successful, takes care of updating
            /// statistics.
            /// </summary>
            /// <param name="road">Road that contains this lane</param>
            /// <param name="carInFront">
            /// If successful, returns the car that was last in the lane until now (<c>null</c> if there was no car);
            /// undefined otherwise
            /// </param>
            /// <returns><c>true</c> if the car was successfully placed, otherwise <c>false</c></returns>
            public bool TryGetOn(Road road, Car car, out Car carInFront)
            {
                carInFront = lastCar;
                if (firstCar == null)
                    firstCar = car;
                else if (FreeSpace(road.Length) < car.Length)
                    return false;
                else
                    lastCar.SetCarBehind(road, car);
                lastCar = car;
                Time time = road.statistics.CarGotOn(car.Id);
                arriveTimes.Enqueue(time);
                return true;
            }

            /// <summary>
            /// Tries to remove a given car from the end of the lane. If successful, takes care of updating statistics.
            /// </summary>
            /// <param name="road">Road that contains this lane</param>
            /// <param name="travelDuration">
            /// If successful, returns the simulation time at which the car arrived at the lane; undefined otherwise
            /// </param>
            /// <returns><c>true</c> if the car was successfully removed, otherwise <c>false</c></returns>
            public bool TryGetOff(Road road, Car car, out Time travelDuration)
            {
                if (car != firstCar)
                {
                    travelDuration = default;
                    return false;
                }
                firstCar = firstCar.CarBehind;
                if (firstCar == null)
                    lastCar = null;
                else
                    firstCar.RemoveCarInFront(road);
                travelDuration = road.statistics.CarGotOff(car.Id, arriveTimes.Dequeue());
                return true;
            }

            /// <summary>
            /// Iterates through all cars in the lane and applies a given action onto them.
            /// </summary>
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

        /// <summary>
        /// Interface describing statistics collected by a road.
        /// 
        /// Some data might not be available based on the selected level of statistical detail. In that case, those
        /// fields return a default value (usually <c>null</c>).
        /// </summary>
        public interface IRoadStatistics
        {
            /// <summary>
            /// Unique ID of the road
            /// </summary>
            public int RoadId { get; }
            /// <summary>
            /// Records each car that passes through the road together with its arrival and leaving times
            /// </summary>
            public IReadOnlyList<Timestamp<CarPassage>> CarLog { get; }
            /// <summary>
            /// Periodically records throughput metrics of the road.
            /// </summary>
            public IReadOnlyList<Timestamp<Throughput>> ThroughputLog { get; }
        }

        /// <summary>
        /// Captures a car's passage through a road.
        /// </summary>
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

        /// <summary>
        /// Captures a road's throughput metrics at a single moment.
        /// </summary>
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
            private Item<List<Timestamp<CarPassage>>> carLog = new(DetailLevel.Medium, new());
            private Item<List<Timestamp<Throughput>>> throughputLog = new(DetailLevel.High, new());

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

            /// <summary>
            /// Registers that a car arrived at the road.
            /// </summary>
            /// <returns>Current simulation time</returns>
            public Time CarGotOn(int carId)
            {
                return clock.Time;
            }

            /// <summary>
            /// Registers that a car left the road.
            /// </summary>
            /// <returns>Duration of the car's passage through the road</returns>
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
