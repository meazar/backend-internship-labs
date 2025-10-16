// Services/IBookable.cs

using TripManagementSystem.Models;

namespace TripManagementSystem.Services
{
     // Demonstrates use of an interface. TripManager will implement this.
    public interface IBookable
    {
        Booking BookTrip(int userId, int tripId);
        void CancelBooking(int userId, int bookingId);
    }
}