using System;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Road : Edge<Coords, int>
    {
        private Queue<Car> cars = new Queue<Car>();
        private Car lastCar = null;

        public Meters Length { get; }
        public MetersPerSecond MaxSpeed { get; }
        public Crossroad Destination { get => (Crossroad)ToNode; }

        public Road(int id, Crossroad from, Crossroad to, Meters length, MetersPerSecond maxSpeed)
            : base(id, from, to, (length / maxSpeed).Weight())
        {
            Length = length;
            MaxSpeed = maxSpeed;
        }

        public bool GetOn(Car car, out Car carInFront)
        {
            carInFront = lastCar;
            if (lastCar != null && lastCar.DistanceRear < car.Length)
                return false;
            cars.Enqueue(car);
            lastCar = car;
            return true;
        }

        public bool GetOff(Car authentication)
        {
            if (authentication != cars.Peek())
                return false;
            cars.Dequeue();
            if (cars.Count == 0)
                lastCar = null;
            else
                cars.Peek().RemoveCarInFront(this);
            return true;
        }

        public void Tick(Seconds time)
        {
            foreach (Car car in cars)
                car.Tick(time);
        }
    }
}
