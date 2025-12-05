using APIBusSeatBookingManagement.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIBusSeatBookingManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff,Admin")]
    public class StaffController : ControllerBase
    {

        private readonly AppDbContext _context;

        public StaffController(AppDbContext context)
        {
            _context = context;

        }


        [HttpGet("today-bookings")]
        public async Task<IActionResult> GetTodayBookings()
        {
            var today = DateTime.UtcNow.Date;

            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Bus)
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Route)
                .Include(b => b.Seat)
                .Where(b => b.Schedule.DepartureTime.Date == today)
                .OrderBy(b => b.Schedule.DepartureTime)
                .Select(b => new
                {
                    b.BookingId,
                    UserName = b.User.FullName,
                    BusNumber = b.Schedule.Bus.BusNumber,
                    Route = $"{b.Schedule.Route.FromLocation} → {b.Schedule.Route.ToLocation}",
                    DepartureTime = b.Schedule.DepartureTime,
                    b.Seat.SeatNumber,
                    b.Status
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpPost("check-in/{bookingId}")]
        public async Task<IActionResult> CheckInPassenger(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Schedule)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
                return NotFound("Booking not found");

            if (booking.Status != "Booked")
                return BadRequest($"Booking status is {booking.Status}, cannot check in");

            if (booking.Schedule.DepartureTime < DateTime.UtcNow)
                return BadRequest("Bus has already departed");


            booking.Status = "Completed";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Passenger checked in successfully" });
        }

        [HttpGet("bus-occupancy/{busId}")]
        public async Task<IActionResult> GetBusOccupancy(int busId, [FromQuery] DateTime date)
        {
            var schedules = await _context.Schedule
                .Include(s => s.Seats)
                .Include(s => s.Route)
                .Where(s => s.BusId == busId && s.DepartureTime.Date == date.Date)
                .Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    Route = $"{s.Route.FromLocation} → {s.Route.ToLocation}",
                    DepartureTime = s.DepartureTime,
                    TotalSeats = s.Seats.Count,
                    BookedSeats = s.Seats.Count(seat => seat.IsBooked),
                    AvailableSeats = s.Seats.Count(seat => !seat.IsBooked),
                    OccupancyRate = (double)s.Seats.Count(seat => seat.IsBooked) / s.Seats.Count * 100
                })
                .ToListAsync();

            return Ok(schedules);
        }
    }

}
