// Models/Booking.cs

namespace TripManagementSystem.Models
{
    public class Booking
    {
        private static int _bookingIdSeq = 1;

        public int BookingId { get; }
        public int UserId { get; set; }
        public int TripId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal Price { get; set; }

        // Constructor
        public Booking(int userId, int tripId, decimal price)
        {
            BookingId = System.Threading.Interlocked.Increment(ref _bookingIdSeq);
            UserId = userId;
            TripId = tripId;
            Price = price;
            BookingDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Booking[{BookingId}] User:{UserId} Trip:{TripId} Price:{Price:C} Date(UTC):{BookingDate:yyyy-MM-dd HH:mm}";
        }
    }
}