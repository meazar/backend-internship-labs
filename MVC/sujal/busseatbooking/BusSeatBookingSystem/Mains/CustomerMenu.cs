using BusSeatBookingSystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Mains
{
    public class CustomerMenu
    {
        private readonly AuthService _authService;
        private readonly BookingService _bookingService;

        public CustomerMenu(AuthService authService, BookingService bookingService)
        {
            _authService = authService;
            _bookingService = bookingService;
        }

        public void Show()
        {
            while (_authService.CurrentUser != null)
            {
                Console.WriteLine("\n=== Customer Menu ===");
                Console.WriteLine("1. View Available Schedules");
                Console.WriteLine("2. Search Schedules");
                Console.WriteLine("3. Book a Seat");
                Console.WriteLine("4. View My Bookings");
                Console.WriteLine("5. Cancel Booking");
                Console.WriteLine("6. Logout");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewAvailableSchedules();
                        break;
                    case "2":
                        SearchSchedules();
                        break;
                    case "3":
                        BookSeat();
                        break;
                    case "4":
                        ViewMyBookings();
                        break;
                    case "5":
                        CancelBooking();
                        break;
                    case "6":
                        _authService.LogOut();
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        private void ViewAvailableSchedules(string fromLocation = null, string toLocation = null)
        {
            var schedules = _bookingService.GetAvailableSchedules(fromLocation, toLocation);

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

        private void SearchSchedules()
        {
            Console.Write("From Location: ");
            var from = Console.ReadLine();
            Console.Write("To Location: ");
            var to = Console.ReadLine();

            ViewAvailableSchedules(from, to);
        }

        private void BookSeat()
        {
            Console.Write("Enter Schedule ID: ");
            if (!int.TryParse(Console.ReadLine(), out int scheduleId))
            {
                Console.WriteLine("Invalid Schedule ID!");
                return;
            }

            var seats = _bookingService.GetAvailableSeats(scheduleId);
            if (seats.Count == 0)
            {
                Console.WriteLine("No available seats for this schedule!");
                return;
            }

            Console.WriteLine("\n=== Available Seats ===");
            foreach (var seat in seats)
            {
                Console.WriteLine($"Seat ID: {seat.SeatId}, Number: {seat.SeatNumber}");
            }

            Console.Write("Enter Seat ID: ");
            if (!int.TryParse(Console.ReadLine(), out int seatId))
            {
                Console.WriteLine("Invalid Seat ID!");
                return;
            }

            try
            {
                var bookingId = _bookingService.CreateBooking(_authService.CurrentUser.UserId, scheduleId, seatId);
                Console.WriteLine($"Booking successful! Booking ID: {bookingId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Booking failed: {ex.Message}");
            }
        }

        private void ViewMyBookings()
        {
            var bookings = _bookingService.GetUserBookings(_authService.CurrentUser.UserId);

            Console.WriteLine("\n=== My Bookings ===");
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking.BookingId}");
                Console.WriteLine($"Route: {booking.FromLocation} -> {booking.ToLocation}");
                Console.WriteLine($"Departure: {booking.DepartureTime}");
                Console.WriteLine($"Seat: {booking.SeatNumber}");
                Console.WriteLine($"Status: {booking.Status}");
                Console.WriteLine($"Booked on: {booking.BookingTime}");
                Console.WriteLine("---------------------------");
            }
        }

        private void CancelBooking()
        {
            Console.Write("Enter Booking ID to cancel: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid Booking ID!");
                return;
            }

            if (_bookingService.CancelBooking(bookingId, _authService.CurrentUser.UserId))
            {
                Console.WriteLine("Booking cancelled successfully!");
            }
            else
            {
                Console.WriteLine("Failed to cancel booking. Please check the Booking ID.");
            }
        }
    }
}

