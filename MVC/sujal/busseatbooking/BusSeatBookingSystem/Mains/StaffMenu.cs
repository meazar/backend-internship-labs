using BusSeatBookingSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Mains
{
    public class StaffMenu
    {
        private readonly AuthService _authService;
        private readonly Admin _adminService;
        private readonly BookingService _bookingService;

        public StaffMenu(AuthService authService, Admin adminService, BookingService bookingService)
        {
            _authService = authService;
            _adminService = adminService;
            _bookingService = bookingService;
        }

        public void Show()
        {
            while (_authService.CurrentUser != null)
            {
                Console.WriteLine("\n=== Staff Menu ===");
                Console.WriteLine("1. View All Bookings");
                Console.WriteLine("2. View Available Schedules");
                Console.WriteLine("3. Logout");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewAllBookings();
                        break;
                    case "2":
                        ViewAvailableSchedules();
                        break;
                    case "3":
                        _authService.LogOut();
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        private void ViewAllBookings()
        {
            var bookings = _adminService.GetAllBooking();

            Console.WriteLine("\n=== All Bookings ===");
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingId}");
                Console.WriteLine($"User: {booking.UserName}");
                Console.WriteLine($"Route: {booking.FromLocation} -> {booking.ToLocation}");
                Console.WriteLine($"Departure: {booking.DepartureTime}");
                Console.WriteLine($"Seat: {booking.SeatNumber}");
                Console.WriteLine($"Status: {booking.Status}");
                Console.WriteLine($"Booked on: {booking.BookingTime}");
                Console.WriteLine("---------------------------");
            }
        }

        private void ViewAvailableSchedules()
        {
            var schedules = _bookingService.GetAvailableSchedules();

            Console.WriteLine("\n=== Available Schedules ===");
            if (schedules.Count == 0)
            {
                Console.WriteLine("No schedules found.");
                return;
            }

            foreach (var schedule in schedules)
            {
                Console.WriteLine($"Schedule ID: {schedule.ScheduleId}");
                Console.WriteLine($"Route: {schedule.FromLocation} -> {schedule.ToLocation}");
                Console.WriteLine($"Bus: {schedule.BusNumber}");
                Console.WriteLine($"Departure: {schedule.DepartureTime}");
                Console.WriteLine($"Arrival: {schedule.ArrivalTime}");
                Console.WriteLine($"Available Seats: {schedule.AvailableSeats}");
                Console.WriteLine($"Price: ${schedule.TicketPrice}");
                Console.WriteLine("---------------------------");
            }
        }
    }
}

