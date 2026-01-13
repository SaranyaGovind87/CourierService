using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int MaxSpeed { get; set; }
        public int Capacity { get; set; }
        public decimal AvailableTime { get; set; } = 0; // When the vehicle returns and is ready

        public Vehicle(int id, int maxSpeed, int capacity)
        {
            Id = id;
            MaxSpeed = maxSpeed;
            Capacity = capacity;
        }
    }
}
