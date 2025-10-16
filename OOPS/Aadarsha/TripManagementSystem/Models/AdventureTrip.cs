// Models/AdventureTrip.cs


namespace TripManagementSystem.Models
{
    // Inherits TripPackage (Inheritance)
    public class AdventureTrip : TripPackage
    {
        public int AdventureLevel { get; set; } // 1 to 5 scale


        // Constructor
        public AdventureTrip(int id, string? name, string? destination, int durationDays, decimal basePrice, int adventureLevel)
            : base(id, name, destination, durationDays, basePrice)
        {
            AdventureLevel = adventureLevel;
        }

        public override decimal CalculatePrice()
        {
            decimal surcharge = BasePrice * (0.1m * AdventureLevel); // 10% per adventure level
            return BasePrice + surcharge;
        }

        public override string ToString()
        {
            return base.ToString() + $" [Adventure Level: {AdventureLevel}] - Total Price: {CalculatePrice():C}";
        }
    }
}