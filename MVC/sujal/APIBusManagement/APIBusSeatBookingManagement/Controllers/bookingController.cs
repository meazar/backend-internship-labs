using APIBusSeatBookingManagement.Database;
using APIBusSeatBookingManagement.Dto;
using APIBusSeatBookingManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APIBusSeatBookingManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class bookingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public bookingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("book")]
        public async Task<IActionResult> CreateBooking(CreateBooking bookingDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            var schedule = await _context.Schedule
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .FirstOrDefaultAsync(s => s.ScheduleId == bookingDto.ScheduleId);


            if(schedule == null)
            {
                return NotFound("Schedule NOt Found");

            }

            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.ScheduleId == bookingDto.ScheduleId
                && s.SeatNumber == bookingDto.SeatNumber
                && !s.IsBooked);


            if(seat == null)
            {
                return BadRequest("Seat is not Available");

            }

            var booking = new Booking
            {

                UserId = userId,
                ScheduleId = schedule.ScheduleId,
                SeatId = seat.SeatId,
                Status = "Booked"
            };


            seat.IsBooked = true;   


            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                BookingId = booking.BookingId,
                Amount = schedule.Price,
                PaymentMethod = "Card",
                paymentTime = DateTime.UtcNow,
            };


            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var response = new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BusNumber = schedule.Bus.BusNumber,
                FromLocation = schedule.Route.FromLocation,
                ToLocation = schedule.Route.ToLocation,
                DepartureTime = schedule.DepartureTime,
                SeatNumber = seat.SeatNumber,
                Price = schedule.Price,
                Status = booking.Status,
                BookingTime = booking.BookingTime,

            };

            return Ok(response);

        }


        [HttpGet("my-bookings")]
        [Authorize]
        public async Task<IActionResult> GetMyBooking()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var bookings = await _context.Bookings
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Bus)
                .Include(s => s.Schedule)
                .ThenInclude(s => s.Route)
                .Include(b => b.Seat)
                .Include(b => b.Payment)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingTime)
                .Select(b => new BookingResponseDto
                {

                    BookingId = b.BookingId,
                    BusNumber = b.Schedule.Bus.BusNumber,
                    FromLocation = b.Schedule.Route.FromLocation,
                    ToLocation = b.Schedule.Route.ToLocation,
                    DepartureTime = b.Schedule.DepartureTime,
                    SeatNumber = b.Seat.SeatNumber,
                    Price = b.Schedule.Price,
                    Status = b.Status,
                    BookingTime = b.BookingTime
                })
                .ToListAsync();
            return Ok(bookings);

        }


        [HttpPost("cancel/{boookingId}")]

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var booking = await _context.Bookings
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.UserId == userId);

            if(booking == null)
            {
                return NotFound("Booking not Found");

            }

            if(booking.Status == "Cancelled")
            {
                return BadRequest("Booking already cancelled");

            }


            booking.Status = "Cancelled";

            booking.Seat.IsBooked = false;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Booking cancelled successfully" });
        }


        [HttpGet("available-seats/{scheduleId}")]
        public async Task<IActionResult> GetAvailableSeats(int scheduleId)
        {
            var availableSeats = await _context.Seats
                .Where(s => s.ScheduleId == scheduleId && !s.IsBooked)
                .Select(s => s.SeatNumber)
                .ToListAsync();

            var schedule = await _context.Schedule
                .Include(s => s.Bus)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null)
                return NotFound("Schedule not found");

            return Ok(new
            {
                ScheduleId = scheduleId,
                busNumber = schedule.Bus.BusNumber,
                TotalSeat = schedule.Bus.TotalSeats,
                AvailableSeats = availableSeats.Count,
                AvailableSeatNumbers = availableSeats
            });
        }
    }
}
