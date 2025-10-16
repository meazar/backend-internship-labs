// Models/CulturalTrip.cs

namespace TripManagementSystem.Models
{
    public class CulturalTrip: TripPackage // Inherits TripPackage (Inheritance)
    {
        public bool GuideIncluded { get; set; }

        // Constructor
        public CulturalTrip(int id, string? name, string? destination, int durationDays, decimal basePrice, bool guidedIncluded)
            : base(id, name, destination, durationDays, basePrice)
        {
            GuideIncluded = guidedIncluded;
        }

        public override decimal CalculatePrice()
        {
            decimal guideFee = GuideIncluded ? 1000m : 0m; // Fixed guide fee if guided tours are included and flat base price if not included
            return BasePrice + guideFee;
        }
        public override string ToString()
        {
            return base.ToString() + $" [Guide: {GuideIncluded}] - Total Price: {CalculatePrice():C}";
        }

    }
}