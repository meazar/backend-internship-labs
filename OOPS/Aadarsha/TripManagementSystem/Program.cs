using System;
using TripManagementSystem.Models;
using TripManagementSystem.Services;
using TripManagementSystem.Exceptions;

namespace TripManagementSystem
{
    // Helper class for fancy centered text
    static class ConsoleHelper
    {
        public static void WriteCentered(string text, ConsoleColor color = ConsoleColor.White, bool newline = true)
        {
            int windowWidth = Console.WindowWidth;
            int leftPadding = Math.Max((windowWidth - text.Length) / 2, 0);

            Console.ForegroundColor = color;
            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            if (newline)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteDivider(char ch = '═')
        {
            Console.WriteLine(new string(ch, Console.WindowWidth));
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Trip Management System - Console";
            var manager = new TripManager();
            manager.SeedSampleData(); // pre-populate trips and sample user

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                ConsoleHelper.WriteDivider();
                ConsoleHelper.WriteCentered("🌍 Trip Management System 🌍", ConsoleColor.Cyan);
                ConsoleHelper.WriteDivider();

                Console.WriteLine();
                ConsoleHelper.WriteCentered("--- Main Menu ---", ConsoleColor.Yellow);
                Console.WriteLine();

                ConsoleHelper.WriteCentered("1. View Available Trips");
                ConsoleHelper.WriteCentered("2. Register User");
                ConsoleHelper.WriteCentered("3. Book a Trip");
                ConsoleHelper.WriteCentered("4. View My Bookings");
                ConsoleHelper.WriteCentered("5. Cancel Booking");
                ConsoleHelper.WriteCentered("6. Exit");

                Console.WriteLine();
                ConsoleHelper.WriteDivider('─');
                ConsoleHelper.WriteCentered("Select option: ", ConsoleColor.Green, newline: false);

                var input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            manager.DisplayAvailableTrips();
                            break;

                        case "2":
                            ConsoleHelper.WriteCentered("Enter name: ", ConsoleColor.Yellow, newline: false);
                            var name = Console.ReadLine() ?? "";

                            ConsoleHelper.WriteCentered("Enter email: ", ConsoleColor.Yellow, newline: false);
                            var email = Console.ReadLine() ?? "";

                            var user = manager.RegisterUser(name, email);
                            ConsoleHelper.WriteCentered($" User registered. UserId: {user.UserId}", ConsoleColor.Green);
                            break;

                        case "3":
                            ConsoleHelper.WriteCentered("Enter your UserId: ", ConsoleColor.Yellow, newline: false);
                            var uidStr = Console.ReadLine();

                            ConsoleHelper.WriteCentered("Enter TripId to book: ", ConsoleColor.Yellow, newline: false);
                            var tidStr = Console.ReadLine();

                            if (!int.TryParse(uidStr, out var uid) || !int.TryParse(tidStr, out var tid))
                            {
                                ConsoleHelper.WriteCentered(" Invalid numeric input.", ConsoleColor.Red);
                                break;
                            }

                            var booking = manager.BookTrip(uid, tid);
                            ConsoleHelper.WriteCentered($" Booking successful. BookingId: {booking.BookingId}, Price: {booking.Price:C}", ConsoleColor.Green);
                            break;

                        case "4":
                            ConsoleHelper.WriteCentered("Enter your UserId: ", ConsoleColor.Yellow, newline: false);
                            var viewUidStr = Console.ReadLine();

                            if (!int.TryParse(viewUidStr, out var viewUid))
                            {
                                ConsoleHelper.WriteCentered(" Invalid UserId.", ConsoleColor.Red);
                                break;
                            }

                            manager.DisplayUserBookings(viewUid);
                            break;

                        case "5":
                            ConsoleHelper.WriteCentered("Enter your UserId: ", ConsoleColor.Yellow, newline: false);
                            var cUidStr = Console.ReadLine();

                            ConsoleHelper.WriteCentered("Enter BookingId to cancel: ", ConsoleColor.Yellow, newline: false);
                            var bIdStr = Console.ReadLine();

                            if (!int.TryParse(cUidStr, out var cUid) || !int.TryParse(bIdStr, out var bId))
                            {
                                ConsoleHelper.WriteCentered(" Invalid numeric input.", ConsoleColor.Red);
                                break;
                            }

                            manager.CancelBooking(cUid, bId);
                            ConsoleHelper.WriteCentered(" Booking cancelled successfully.", ConsoleColor.Green);
                            break;

                        case "6":
                            exit = true;
                            break;

                        default:
                            ConsoleHelper.WriteCentered(" Invalid option. Please try again.", ConsoleColor.Red);
                            break;
                    }
                }
                catch (BookingNotFoundException ex)
                {
                    ConsoleHelper.WriteCentered($" Booking error: {ex.Message}", ConsoleColor.Red);
                }
                catch (InvalidTripException ex)
                {
                    ConsoleHelper.WriteCentered($" Trip error: {ex.Message}", ConsoleColor.Red);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteCentered($" Unexpected error: {ex.Message}", ConsoleColor.Red);
                }

                if (!exit)
                {
                    ConsoleHelper.WriteCentered("\nPress any key to return to menu...", ConsoleColor.DarkGray);
                    Console.ReadKey();
                }
            }

            Console.Clear();
            ConsoleHelper.WriteCentered(" Goodbye! Thanks for using Trip Management System.", ConsoleColor.Cyan);
            ConsoleHelper.WriteDivider();
        }
    }
}
