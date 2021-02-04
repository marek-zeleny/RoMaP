using System;
using System.Collections.Generic;

using RoadTrafficSimulator.ValueTypes;
using DataStructures.Graphs;

namespace RoadTrafficSimulator.Components
{
    class Road : Edge<Coords, int>
    {
        private Car firstCar;
        private Car lastCar;
        private MetersPerSecond maxSpeed;

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
        public Crossroad Destination { get => (Crossroad)ToNode; }

        public Road(int id, Crossroad from, Crossroad to, Meters length, MetersPerSecond maxSpeed)
            : base(id, from, to, (length / maxSpeed).Weight())
        {
            Length = length;
            MaxSpeed = maxSpeed;
        }

        public bool Initialize()
        {
            firstCar = null;
            lastCar = null;
            return true;
        }

        public bool GetOn(Car car, out Car carInFront)
        {
            carInFront = lastCar;
            if (firstCar == null)
                firstCar = car;
            else
            {
                if (lastCar.DistanceRear < car.Length)
                    return false;
                if (!lastCar.SetCarBehind(this, car))
                    return false;
            }
            lastCar = car;
            return true;
        }

        public bool GetOff(Car authentication)
        {
            if (authentication != firstCar)
                return false;
            Car newFirstCar = firstCar.CarBehind;
            if (newFirstCar == null)
                lastCar = null;
            else if (!newFirstCar.RemoveCarInFront(this))
                return false;
            firstCar = newFirstCar;
            return true;
        }

        public void Tick(Seconds time)
        {
            Car current = firstCar;
            // Because the current Car can leave this Road during its Tick(), we need to know the next Car beforehands
            Car next;
            while (current != null)
            {
                next = current.CarBehind;
                current.Tick(time);
                current = next;
            }
        }
    }
}
