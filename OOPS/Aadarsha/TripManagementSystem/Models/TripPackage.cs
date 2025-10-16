// Models/TripPackage.cs

using System;

namespace TripManagementSystem.Models
{
    // Abstract base class demonstrating Abstraction and Polymorphism
    // Derived classes override the CalculatePrice() method

    public abstract class TripPackage
    {
        public int TripId { get; set; }
        public string? Name { get; set; }
        public string? Destination { get; set; }
        public int DurationDays { get; set; }
        public decimal BasePrice { get; set; }

        // constructor
        public TripPackage(int tripId, string? name, string? destination, int durationDays, decimal basePrice)
        {
            TripId = tripId;
            Name = name;
            Destination = destination;
            DurationDays = durationDays;
            BasePrice = basePrice;
        }

        // Implemented differently by derived classes (polymorphism)
        public abstract decimal CalculatePrice();
        public override string ToString()
        {
            return $"[{TripId}] {Name} - {Destination} ({DurationDays} days): {BasePrice:C}";
        }
        


    }
}