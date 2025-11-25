using BusSeatBookingSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Mains
{
    public class AdminMenu
    {
        private readonly AuthService _authService;
        private readonly Admin _adminService;
        private readonly BookingService _bookingService;

        public AdminMenu(AuthService authService, Admin adminService, BookingService bookingService)
        {
            _authService = authService;
            _adminService = adminService;
            _bookingService = bookingService;
        }

        public void Show()
        {
            while (_authService.CurrentUser != null)
            {
                Console.WriteLine("\n=== Admin Menu ===");
                Console.WriteLine("1. Add Bus");
                Console.WriteLine("2. Add Route");
                Console.WriteLine("3. Add Schedule");
                Console.WriteLine("4. View Buses");
                Console.WriteLine("5. View Route");                
                Console.WriteLine("6. View All Bookings");
                Console.WriteLine("7. View Available Schedules");
                Console.WriteLine("8. Logout");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddBus();
                        break;
                    case "2":
                        AddRoute();
                        break;
                    case "3":
                        AddSchedule();
                        break;
                    case "4":
                        GetAllBuses();
                        break;
                    case "5":
                        GetAllRoutes();
                        break;
                    case "6":
                        ViewAllBookings();
                        break;
                    case "7":
                        ViewAvailableSchedules();
                        break;
                    case "8":
                        _authService.LogOut();
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        private void AddBus()
        {
            Console.Write("Bus Number: ");
            var busNumber = Console.ReadLine();
            Console.Write("Bus Type (AC/Non-AC/Sleeper): ");
            var busType = Console.ReadLine();
            Console.Write("Total Seats: ");
            if (!int.TryParse(Console.ReadLine(), out int totalSeats))
            {
                Console.WriteLine("Invalid number of seats!");
                return;
            }

            try
            {
                _adminService.AddBus(busNumber, busType, totalSeats);
                Console.WriteLine("Bus added successfully!");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add bus: {ex.Message}");
                return;
            }
        }

        private void AddRoute()
        {
            Console.Write("From Location: ");
            var from = Console.ReadLine();
            Console.Write("To Location: ");
            var to = Console.ReadLine();
            Console.Write("Distance (km): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal distance))
            {
                Console.WriteLine("Invalid distance!");
                return;
            }
            Console.Write("Duration (minutes): ");
            if (!int.TryParse(Console.ReadLine(), out int duration))
            {
                Console.WriteLine("Invalid duration!");
                return;
            }
            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid price!");
                return;
            }

            try
            {
                _adminService.AddRoute(from, to, distance, duration, price);
                Console.WriteLine("Route added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add route: {ex.Message}");
            }
        }

        private void AddSchedule()
        {
            Console.Write("Bus ID: ");
            if (!int.TryParse(Console.ReadLine(), out int busId))
            {
                Console.WriteLine("Invalid Bus ID!");
                return;
            }
            Console.Write("Route ID: ");
            if (!int.TryParse(Console.ReadLine(), out int routeId))
            {
                Console.WriteLine("Invalid Route ID!");
                return;
            }
            Console.Write("Departure Time (yyyy-mm-dd hh:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime departure))
            {
                Console.WriteLine("Invalid departure time!");
                return;
            }
            Console.Write("Arrival Time (yyyy-mm-dd hh:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime arrival))
            {
                Console.WriteLine("Invalid arrival time!");
                return;
            }

            try
            {
                _adminService.AddSchedule(busId, routeId, departure, arrival);
                Console.WriteLine("Schedule added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add schedule: {ex.Message}");
            }
        }

        private void GetAllBuses()
        {
            var buses = _adminService.GetAllBuses();

            Console.WriteLine("\n=== All Buses ===");
            if(buses.Count == 0)
            {
                Console.WriteLine("No buses found.");

            }
            foreach (var bus in buses)
            {
                Console.WriteLine($"BUS ID: {bus.BusId}");
                Console.WriteLine($"Bus Number: {bus.BusNumber}");
                Console.WriteLine($"Bus Type: {bus.BusType}");
                Console.WriteLine($"Total Seats: {bus.TotalSeats}");
                Console.WriteLine("---------------------------");
            }
        }

        private void GetAllRoutes()
        {
            var routes = _adminService.GetAllRoutes();
            Console.WriteLine("\n=== All Routes ===");
            if (routes.Count == 0)
            {
                Console.WriteLine("No routes found.");
                return;
            }
            foreach(var route in routes)
            {
                Console.WriteLine($"Route ID: {route.RouteId}");
                Console.WriteLine($"From: {route.FromLocation}");
                Console.WriteLine($"To: {route.ToLocation}");
                Console.WriteLine($"Distance: {route.DistanceKm} km");
                Console.WriteLine($"Duration: {route.DurationMinutes} minutes");
                Console.WriteLine($"Price: ${route.TicketPrice}");
                Console.WriteLine("---------------------------");
            }

        }

        public void ViewAllSchedules()
        {
            var schedules = _adminService.GetAllSchedules();
            Console.WriteLine("\n=== All Schedules ===");
            if (schedules.Count == 0)
            {
                Console.WriteLine("No schedules found.");
                return;
            }
            foreach (var schedule in schedules)
            {
                Console.WriteLine($"Schedule ID: {schedule.ScheduleId}");
                Console.WriteLine($"Bus: {schedule.BusNumber}");
                Console.WriteLine($"Route: {schedule.FromLocation} -> {schedule.ToLocation}");
                Console.WriteLine($"Departure: {schedule.DepartureTime}");
                Console.WriteLine($"Arrival: {schedule.ArrivalTime}");
                Console.WriteLine($"Available Seats: {schedule.AvailableSeats}");
                Console.WriteLine($"Price: ${schedule.TicketPrice}");
                Console.WriteLine("---------------------------");
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

