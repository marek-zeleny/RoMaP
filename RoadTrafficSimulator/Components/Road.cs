using System;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Road : Edge<Coords, int>
    {
        public const int maxLaneCount = 3;

        private MetersPerSecond maxSpeed;
        private Lane[] lanes;
        private int laneCount;

        public Meters Length { get; }
        public MetersPerSecond MaxSpeed
        {
            get => maxSpeed;
            set
            {
                if (value < 1)
                    maxSpeed = 1.MetersPerSecond();
                else
                    maxSpeed = value;
                SetWeight((Length / maxSpeed).Weight());
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

        public Road(int id, Crossroad from, Crossroad to, Meters length, MetersPerSecond maxSpeed)
            : base(id, from, to, (length / maxSpeed).Weight())
        {
            Length = length;
            MaxSpeed = maxSpeed;
            lanes = new Lane[maxLaneCount];
            LaneCount = 1;
            lanes[0].Initialise();
        }

        public bool Initialise()
        {
            for (int i = 0; i < LaneCount; i++)
                lanes[i].Initialise();
            return true;
        }

        public bool TryGetOn(Car car, out Car carInFront)
        {
            int maxIndex = 0;
            Meters maxSpace = lanes[0].FreeSpace(Length);
            for (int i = 1; i < LaneCount; i++)
            {
                Meters space = lanes[i].FreeSpace(Length);
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
                if (lanes[i].TryGetOff(this, car))
                    return;
            }
            throw new ArgumentException("The car must be first in a lane to get off the road.", nameof(car));
        }

        public void Tick(Seconds time)
        {
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car => car.Tick(time));
            for (int i = 0; i < LaneCount; i++)
                lanes[i].ForAllCars(car => car.FinishCrossingRoads(time));
        }

        private struct Lane
        {
            private Car firstCar;
            private Car lastCar;

            public void Initialise()
            {
                firstCar = null;
                lastCar = null;
            }

            public Meters FreeSpace(Meters length)
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
                return true;
            }

            public bool TryGetOff(Road road, Car car)
            {
                if (car != firstCar)
                    return false;
                firstCar = firstCar.CarBehind;
                if (firstCar == null)
                    lastCar = null;
                else
                    firstCar.RemoveCarInFront(road);
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
    }
}
