// Services/TripManager.cs

using System;
using System.Collections.Generic;
using System.Linq;

using TripManagementSystem.Models;
using TripManagementSystem.Exceptions;

namespace TripManagementSystem.Services
{
    // TripManager handles trips, users and bookings and implements IBookable.

    public class TripManager: IBookable
    {
        // List to store trips users, packages and a dictionary for user->bookings
        private List<TripPackage> _trips = new List<TripPackage>();
        private List<User> _users = new List<User>();
        private Dictionary<int, List<Booking>> _userBookings = new Dictionary<int, List<Booking>>();

        // Array - static destinations available
        private readonly string[] _destinations = { "Pokhara", "Mustang", "Kathmandu", "Chitwan", "Bhaktapur" };

        // Register a user (returns the created user)
        public User RegisterUser(string name, string email)
        {
            var user = new User(name, email);
            _users.Add(user);

            // create an empty booking
            _userBookings[user.UserId] = new List<Booking>();
            return user;
        }

        // Seed sample data
        public void SeedSampleData()
        {
            // Create sample trips
            _trips.Add(new AdventureTrip(1, "Annapurna Base Camp Trek", "Pokhara", 10, 500m, adventureLevel: 4));
            _trips.Add(new CulturalTrip(2, "Heritage Tour of Kathmandu", "Kathmandu", 3, 150m, guidedIncluded: true));
            _trips.Add(new AdventureTrip(3, "Upper Mustang Expedition", "Mustang", 8, 1000m, adventureLevel: 5));
            _trips.Add(new CulturalTrip(4, "Chitwan Jungle Safari", "Chitwan", 2, 300m, guidedIncluded: false));

            // Create sample user
            var sampleUser = new User("Aadarsha", "aad@example.com");
            _users.Add(sampleUser);
            _userBookings[sampleUser.UserId] = new List<Booking>();
        }

        public void DisplayAvailableTrips()
        {
            Console.WriteLine("Available Trips: ");
            foreach (var trip in _trips)
            {
                Console.WriteLine(trip);
            }
        }

        // IBookable implementation
        public Booking BookTrip(int userId, int tripId)
        {
            // Validate user
            var user = _users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found.");

            // Find trip
            var trip = _trips.FirstOrDefault(t => t.TripId == tripId);
            if (trip == null)
                throw new InvalidTripException(tripId, $"Trip with ID {tripId} not found.");

            // Calculate pricing using polymorphoic method
            var price = trip.CalculatePrice();

            // Create booking and add to dictionary
            var booking = new Booking(userId, tripId, price);
            if (!_userBookings.ContainsKey(userId))
                _userBookings[userId] = new List<Booking>();

            _userBookings[userId].Add(booking);
            return booking;
        }

        public void CancelBooking(int userId, int bookingId)
        {
            if (!_userBookings.ContainsKey(userId))
                throw new BookingNotFoundException(bookingId, $"User with ID {userId} has no bookings.");

            var list = _userBookings[userId];
            var target = list.FirstOrDefault(b => b.BookingId == bookingId);
            if (target == null)
                throw new BookingNotFoundException(bookingId, $"Booking with Id {bookingId} not found for user {userId}.");
            // Remove booking
            list.Remove(target);
        }

        // Display bookings for a user
        public void DisplayUserBookings(int userId)
        {
            if (!_userBookings.ContainsKey(userId) || !_userBookings[userId].Any())
            {
                Console.WriteLine($"No bookings found for user ID {userId}.");
                return;
            }

            Console.WriteLine($"\nBookings for User ID {userId}:");
            foreach (var booking in _userBookings[userId])
            {
                Console.WriteLine(booking);
            }
        }

        // Helper: get trips as array 
        public TripPackage[] GetAllTrips()
        {
            return _trips.ToArray();
        }

        // Expose users list (read-only copy)
        public List<User> GetAllUsers() => new List<User>(_users);
    }
}