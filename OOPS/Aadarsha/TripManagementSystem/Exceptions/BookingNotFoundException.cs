// Exceptions/BookingNotFoundException.cs

using System;

namespace TripManagementSystem.Exceptions
{
    public class BookingNotFoundException: Exception
    {
        public int BookingId { get; }

        public BookingNotFoundException(int bookingId, string message) : base(message)
        {
            BookingId = bookingId;
        }
    }
}