using APIBusSeatBookingManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace APIBusSeatBookingManagement.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Routes> Routes { get; set; }

        public DbSet<Schedule> Schedule { get; set; }

        public DbSet<Seat> Seats { get; set; }
        
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }

       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Bus)
                .WithMany(b => b.Schedules)
                .HasForeignKey(s => s.BusId);

            //modelBuilder.Entity<Schedule>()
            //    .HasOne(s => s.Route)
            //    .WithMany(r => r.Schedules)
            //    .HasForeignKey(s => s.RouteId);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Schedule)
                .WithMany(sch => sch.Seats)
                .HasForeignKey(s => s.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Schedule)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Seat)
                .WithMany(s => s.Booking)
                .HasForeignKey(b => b.SeatId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Bus>()
                .HasIndex(b => b.BusNumber)
                .IsUnique();

            modelBuilder.Entity<Seat>()
                .HasIndex(s => new { s.ScheduleId, s.SeatNumber })
                .IsUnique();

            SeedData.Seed(modelBuilder);

            
        }



    }
}
