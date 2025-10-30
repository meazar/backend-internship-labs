// Exceptions/InvalidTripException.cs

using System;

namespace TripManagementSystem.Exceptions
{
    // Custom exception for invalid trip operations
    public class InvalidTripException : Exception
    {
        public int TripId { get; }

        // Constructor to initialize the exception with a message and trip ID
        public InvalidTripException(int tripId, string message) : base(message)
        {
            TripId = tripId;
        }
    }
}