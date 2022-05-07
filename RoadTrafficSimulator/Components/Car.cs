using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Diagnostics;

using RoadTrafficSimulator.ValueTypes;
using RoadTrafficSimulator.Statistics;

namespace RoadTrafficSimulator.Components
{
    /// <summary>
    /// Represents a single car in the simulation.
    /// </summary>
    class Car
    {
        #region static

        public static readonly Distance minDistanceBetweenCars = 1.Metres();

        private static int nextId = 0;
        private static readonly Acceleration deceleration = 4.MetresPerSecondPerSecond();
        private static readonly Acceleration acceleration = 1600.MillimetresPerSecondPerSecond();
        private static readonly Time reactionTime = 1.Seconds();

        /// <summary>
        /// Calculates optimal speed of a car based on how much free space it has and how fast is the car in front of it
        /// travelling.
        /// </summary>
        /// <returns>Non-negative speed value</returns>
        private static Speed CalculateOptimalSpeed(Distance freeSpace, Speed carInFrontSpeed)
        {
            // Uses the following formula:
            // v = sqrt((a_d t_r)^2 + 2 a_d (s_d - d) + v_f^2) - a_d t_r
            // For more details and derivation of this formula see the programming documentation
            // Has to be calculated in integers instead of the type system because of non-standard intermediate units
            // (mm^2 / s^2), also need to use 64-bit integers because of large intermediate results
            // All defined multiplications and divisions should be done within the type system or use the precision
            // constants for correct conversion

            long convertCoef = (Speed.precision * Speed.precision) / (Acceleration.precision * Distance.precision);
            long x = (long)deceleration * (long)freeSpace * convertCoef; // a_d (s_d - d)
            long v_f = carInFrontSpeed;
            long v;
            if (v_f == 0)
            {
                // If v_f = 0 (the car approaches a crossroad or the car in front doesn't move), we also set t_r = 0
                // v = sqrt(2 a_d (s_d - d))
                v = (long)Math.Sqrt(2 * x);
            }
            else
            {
                long v_d = deceleration * reactionTime; // a_d * t_r
                v = (long)Math.Sqrt(v_d * v_d + 2 * x + v_f * v_f) - v_d;
            }
            Debug.Assert(v >= 0);
            return new Speed((int)v);
        }

        #endregion static

        private readonly INavigation navigation;
        private readonly CarStatistics statistics;
        private Distance distance;
        private bool newRoad;

        /// <summary>
        /// Unique ID within all cars in the simulation
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// <c>true</c> if the car has finished its travel, otherwise <c>false</c>
        /// </summary>
        public bool Finished { get; private set; }
        public Distance Length { get; }
        public Speed CurrentSpeed { get; private set; }
        /// <summary>
        /// Distance of the car's rear from the beginning of the road it's currently on; can be negative
        /// </summary>
        public Distance DistanceRear { get => distance - Length; }
        /// <summary>
        /// Car that is directly in front of this one on the same road; <c>null</c> if this car is the first on a road
        /// </summary>
        private Car CarInFront { get; set; }
        /// <summary>
        /// Car that is directly behind this one on the same road; <c>null</c> if this car is the last on a road
        /// </summary>
        public Car CarBehind { get; private set; }
        /// <summary>
        /// Statistics collected by the car during the simulation
        /// </summary>
        public ICarStatistics Statistics { get => statistics; }

        /// <summary>
        /// Creates a new car with specified length and a given navigation.
        /// </summary>
        /// <param name="collector">Statistics collector necessary to collect this car's statistics</param>
        public Car(Distance length, INavigation navigation, StatisticsCollector collector)
        {
            Id = nextId++;
            Finished = false;
            Length = length;
            this.navigation = navigation;
            statistics = new CarStatistics(collector, navigation.Clock,
                Id, navigation.RemainingDuration, navigation.CurrentRoad.Id);
        }

        #region methods

        /// <summary>
        /// Tries to release the car onto the simulation map.
        /// </summary>
        /// <returns><c>true</c> if the car was successfully released, otherwise <c>false</c></returns>
        public bool Initialise()
        {
            if (!navigation.CurrentRoad.TryGetOn(this, out Car newCarInFront))
                return false;
            CarInFront = newCarInFront;
            return true;
        }

        /// <summary>
        /// Simulates the car's movement for a given time duration and updates statistics accordingly.
        /// After all cars are updated in a tick, the method <c>AfterTick()</c> must be called before another tick.
        /// </summary>
        public void Tick(Time time)
        {
            // If the car already crossed from a different road during this tick, do nothing
            if (newRoad)
                return;
            Distance drivenDistance = Move(time);
            CurrentSpeed = drivenDistance / time;
            Debug.Assert(CurrentSpeed >= 0);
            statistics.Update(drivenDistance, CurrentSpeed);
            TryFinishDrive();
        }

        /// <summary>
        /// Must be called each time after all cars are updated in one simulation tick. Performs clean-up operations.
        /// </summary>
        public void AfterTick()
        {
            newRoad = false;
        }

        /// <summary>
        /// Puts a new car behind this one.
        /// </summary>
        /// <param name="road">The road the car is currently on; used for safety check</param>
        public void SetCarBehind(Road road, Car car)
        {
            Debug.Assert(road == navigation.CurrentRoad);
            CarBehind = car;
        }

        /// <summary>
        /// Removes the car in front of this one.
        /// </summary>
        /// <param name="road">The road the car is currently on; used for safety check</param>
        public void RemoveCarInFront(Road road)
        {
            Debug.Assert(road == navigation.CurrentRoad);
            CarInFront = null;
        }

        /// <summary>
        /// Moves the car for a given amount of time.
        /// </summary>
        /// <returns>Distance travelled by the car</returns>
        private Distance Move(Time time)
        {
            if (CarInFront == null)
                return ApproachCrossroad(time);
            else
                return ApproachCar(time);
        }

        /// <summary>
        /// Approaches the car directly in front of this one with an optimal (safe) speed, for a given amount of time.
        /// </summary>
        /// <returns>Distance travelled by the car</returns>
        private Distance ApproachCar(Time time)
        {
            Distance freeSpace = CarInFront.DistanceRear - distance - minDistanceBetweenCars;
            Speed speed = CalculateOptimalSpeed(freeSpace, CarInFront.CurrentSpeed);

            Speed maxSpeed = CurrentSpeed + acceleration * time;
            if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                maxSpeed = navigation.CurrentRoad.MaxSpeed;
            if (speed > maxSpeed)
                speed = maxSpeed;

            Distance travelledDistance = speed * time;
            // If the car in front is not moving, the optimal speed calculation may overshoot
            if (CarInFront.CurrentSpeed > 0)
                Debug.Assert(travelledDistance <= freeSpace);
            else if (travelledDistance > freeSpace)
                travelledDistance = freeSpace;
            distance += travelledDistance;
            return travelledDistance;
        }

        /// <summary>
        /// Approaches the crossroad at the end of the current road if no car is in front, for a given amount of time.
        /// If the crossroad is reached, tries to continue to the next road given by the navigation.
        /// </summary>
        /// <returns>Distance travelled by the car</returns>
        private Distance ApproachCrossroad(Time time)
        {
            Distance freeSpace = navigation.CurrentRoad.Length - distance;
            Distance travelledDistance;
            Speed maxSpeed = CurrentSpeed + acceleration * time;
            if (maxSpeed > navigation.CurrentRoad.MaxSpeed)
                maxSpeed = navigation.CurrentRoad.MaxSpeed;
            Time expectedArrival = freeSpace / maxSpeed;
            bool freeToGo = navigation.NextRoad != null &&
                navigation.CurrentRoad.Destination.ActiveCrossingAlgorithm.CanCross(
                    this, navigation.CurrentRoad.Id, navigation.NextRoad.Id, expectedArrival);
            if (freeToGo)
            {
                travelledDistance = maxSpeed * time;
                distance += travelledDistance;
                if (distance >= navigation.CurrentRoad.Length)
                {
                    Time remainingTime = (distance - navigation.CurrentRoad.Length) / maxSpeed;
                    distance = navigation.CurrentRoad.Length;
                    travelledDistance = freeSpace + TryCrossToNextRoad(remainingTime);
                }
            }
            else if (freeSpace == 0)
            {
                travelledDistance = new Distance(0);
            }
            else
            {
                Speed speed = CalculateOptimalSpeed(freeSpace, 0.MetresPerSecond());
                if (speed > maxSpeed)
                    speed = maxSpeed;
                travelledDistance = speed * time;
                if (travelledDistance > freeSpace)
                    travelledDistance = freeSpace;
                distance += travelledDistance;
            }
            return travelledDistance;
        }

        /// <summary>
        /// Tries to cross to the next road given by the navigation. If successful, continues to travel on the next road
        /// for the given remaining time duration.
        /// </summary>
        /// <returns>Distance travelled by the car</returns>
        private Distance TryCrossToNextRoad(Time remainingTime)
        {
            if (!navigation.NextRoad.TryGetOn(this, out Car newCarInFront))
                return 0.Metres();

            newRoad = true;
            navigation.CurrentRoad.Destination.ActiveCrossingAlgorithm.CarCrossed(
                this, navigation.CurrentRoad.Id, navigation.NextRoad.Id);
            navigation.CurrentRoad.GetOff(this);
            navigation.MoveToNextRoad();
            statistics.MovedToNextRoad(navigation.CurrentRoad.Id);
            distance = 0.Metres();
            CarInFront = newCarInFront;
            CarBehind = null;
            Distance travelledDistance = Move(remainingTime);
            return travelledDistance;
        }

        /// <summary>
        /// Checks if the car has finished its drive and eventually performs clean-up operations.
        /// </summary>
        private void TryFinishDrive()
        {
            if (navigation.NextRoad == null && distance == navigation.CurrentRoad.Length)
            {
                Finished = true;
                CurrentSpeed = 0.MetresPerSecond();
                navigation.CurrentRoad.GetOff(this);
                statistics.Finish();
            }
        }

        #endregion methods

        #region subclasses

        /// <summary>
        /// Interface describing statistics collected by a car.
        /// 
        /// Some data might not be available based on the selected level of statistical detail. In that case, those
        /// fields return a default value (usually <c>null</c>).
        /// </summary>
        public interface ICarStatistics
        {
            /// <summary>
            /// Unique ID of the car
            /// </summary>
            public int CarId { get; }
            /// <summary>
            /// Simulation time when the car was created.
            /// </summary>
            public Time StartTime { get; }
            /// <summary>
            /// Simulation time when the car reached its destination. If the car has not yet finished, the value is
            /// zero.
            /// </summary>
            public Time FinishTime { get; }
            /// <summary>
            /// Total distance travelled by the car.
            /// </summary>
            public Distance Distance { get; }
            /// <summary>
            /// Duration of the drive calculated by the navigation at the beginning of the trip.
            /// </summary>
            public Time ExpectedDuration { get; }
            /// <summary>
            /// Actual duration of the drive so far.
            /// </summary>
            public Time Duration { get; }
            /// <summary>
            /// Records each road and time the car got on that road.
            /// </summary>
            public IReadOnlyList<Timestamp<int>> RoadLog { get; }
            /// <summary>
            /// Periodically records the car's speed.
            /// </summary>
            public IReadOnlyList<Timestamp<Speed>> SpeedLog { get; }
        }

        private class CarStatistics : StatisticsBase, ICarStatistics
        {
            private Item<Time> startTime = new(DetailLevel.Low);
            private Item<Time> finishTime = new(DetailLevel.Low);
            private Item<Distance> distance = new(DetailLevel.Low, 0.Metres());
            private Item<Time> expectedDuration = new(DetailLevel.Low);
            private Item<List<Timestamp<int>>> roadLog = new(DetailLevel.Medium, new());
            private Item<List<Timestamp<Speed>>> speedLog = new(DetailLevel.High, new());

            public int CarId { get; }
            public Time StartTime { get => startTime; }
            public Time FinishTime { get => finishTime; }
            public Distance Distance { get => distance; }
            public Time ExpectedDuration { get => expectedDuration; }
            public Time Duration
            {
                get
                {
                    if (!finishTime.IsActive || !startTime.IsActive)
                        return default;
                    else if (FinishTime > StartTime)
                        return FinishTime - StartTime;
                    else
                        return clock.Time - StartTime;
                }
            }
            public IReadOnlyList<Timestamp<int>> RoadLog { get => roadLog.Get(); }
            public IReadOnlyList<Timestamp<Speed>> SpeedLog { get => speedLog.Get(); }

            public CarStatistics(StatisticsCollector collector, IClock clock,
                int CarId, Time expectedDuration, int firstRoadId)
                : base(collector, typeof(Car), clock)
            {
                this.CarId = CarId;
                this.expectedDuration.Set(expectedDuration);
                startTime.Set(clock.Time);
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, firstRoadId));
            }

            public void Update(Distance addedDistance, Speed speed)
            {
                distance.Set(Distance + addedDistance);
                speedLog.Get()?.Add(new Timestamp<Speed>(clock.Time, speed));
            }

            public void MovedToNextRoad(int roadId)
            {
                roadLog.Get()?.Add(new Timestamp<int>(clock.Time, roadId));
            }

            public void Finish()
            {
                finishTime.Set(clock.Time);
            }

            public override void Serialise(Utf8JsonWriter writer)
            {
                static int SerialiseTime(Time t) => t;
                static int SerialiseDistance(Distance d) => d;
                static void SerialiseRoadId(Utf8JsonWriter writer, int id) => writer.WriteNumber("roadId", id);
                static void SerialiseSpeed(Utf8JsonWriter writer, Speed s) => writer.WriteNumber("speed", s);

                writer.WriteStartObject();
                writer.WriteNumber("id", CarId);

                SerialiseIntItem(writer, startTime, nameof(startTime), SerialiseTime);
                SerialiseIntItem(writer, finishTime, nameof(finishTime), SerialiseTime);
                SerialiseIntItem(writer, expectedDuration, nameof(expectedDuration), SerialiseTime);
                SerialiseIntItem(writer, distance, nameof(distance), SerialiseDistance);
                SerialiseTimestampListItem(writer, roadLog, nameof(roadLog), SerialiseRoadId);
                SerialiseTimestampListItem(writer, speedLog, nameof(speedLog), SerialiseSpeed);

                writer.WriteEndObject();
            }
        }

        #endregion subclasses
    }
}
