using APIBusSeatBookingManagement.Model;
using APIBusSeatBookingManagement.Service;
using Microsoft.EntityFrameworkCore;

namespace APIBusSeatBookingManagement.Database
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var PasswordService = new PasswordService();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Admin User",
                    Email = "admin@busbooking.com",
                    Password = PasswordService.HashPassword("Admin@123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 2,
                    FullName = "Staff User",
                    Email = "Staff@busbooking.com",
                    Password = PasswordService.HashPassword("Staff123"),
                    Role="Staff",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserId = 3,
                    FullName = "Customer User",
                    Email = "Customer@busbooking.com",
                    Password = PasswordService.HashPassword("User123"),
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<Bus>().HasData(
                new Bus { BusId = 1, BusNumber = "KA01AB1234", BusType = "AC", TotalSeats = 40 },
                new Bus { BusId = 2, BusNumber = "MD02DB5237", BusType = "Non-Ac", TotalSeats = 50 },
                new Bus { BusId = 3, BusNumber = "NP02DB8547", BusType = "Sleeper", TotalSeats = 30 }
                );

            modelBuilder.Entity<Routes>().HasData(
                new Routes { RouteId = 1, FromLocation = "Kathmandu", ToLocation = "Pokhara", DistanceKm = 310, DurationMinutes = 300 },
                new Routes { RouteId = 2, FromLocation = "Daran", ToLocation = "Pokhara", DistanceKm = 310, DurationMinutes = 300 },
                new Routes { RouteId = 3, FromLocation = "Kathmandu", ToLocation = "chitwan", DistanceKm = 310, DurationMinutes = 300 }
                );
        }
    }
}
