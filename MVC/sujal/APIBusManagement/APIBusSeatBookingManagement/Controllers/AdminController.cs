using APIBusSeatBookingManagement.Database;
using APIBusSeatBookingManagement.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIBusSeatBookingManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {

        private readonly AppDbContext _context;
        


        public AdminController(AppDbContext context)
        {
            _context = context;

        }      


        [HttpPost("routes")]
        public async Task<IActionResult> AddRoute([FromBody] Routes routes)
        {
            _context.Routes.Add(routes);

            await _context.SaveChangesAsync();
            return Ok(routes);

        }


        [HttpPost("schedule")]
        //[Authorize(Roles = "Admin,Staff")]
        [Authorize]

        public async Task<IActionResult> AddSchedule([FromBody] Schedule schedules)
        {

            var bus = await _context.Buses.FindAsync(schedules.BusId);

            if(bus == null)
            {
                return NotFound("Bus is Not Found");
            }
            var route = await _context.Routes.FindAsync(schedules.RouteId);
            if(route == null)
            {
                return NotFound();

            }

            _context.Schedule.Add(schedules);
            await _context.SaveChangesAsync();



            var seats = new List<Seat>();
            for(int i =1; i <= bus.TotalSeats; i++)
            {
                seats.Add(new Seat
                {
                    ScheduleId = schedules.ScheduleId,
                    SeatNumber = $"A{i}",
                    IsBooked = false,
                });
            }
            _context.Seats.AddRange(seats);

            await _context.SaveChangesAsync();
            return Ok(schedules);
        }



        [HttpGet("all-bookings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBooking()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Bus)
                .Include(b => b.Schedule)
                .ThenInclude(s => s.Route)
                 .Include(b => b.Seat)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.BookingTime)
                .Select(b => new
                {
                    b.BookingId,
                    UserName = b.User.FullName,
                    BusNumber = b.Schedule.Bus.BusNumber,
                    Route = $"{b.Schedule.Route.FromLocation} => {b.Schedule.Route.ToLocation}",
                    b.Seat.SeatNumber,
                    b.Schedule.Price,
                    b.Status,
                    b.BookingTime,
                    Payment = b.Payment != null ? new
                    {
                        b.Payment.PaymentMethod,
                        b.Payment.Amount,
                        b.Payment.paymentTime


                    } : null

                })
                .ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("revenue")]

        public async Task<IActionResult> GetRevenueReport([FromBody] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var query = _context.Payments.AsQueryable();
            if (fromDate.HasValue)
            {
                query = query.Where(p =>p.paymentTime >= fromDate.Value);
            }
            if(toDate.HasValue)
            {
                query = query.Where(p => p.paymentTime <= toDate.Value);
            }
            var revenue = await query
                .GroupBy(p => new { p.paymentTime.Date, p.PaymentMethod })
                .Select(g => new
                {
                    Date = g.Key.Date,
                    PaymentMethod = g.Key.PaymentMethod,
                    TotalAmount = g.Sum(p => p.Amount),
                    Count = g.Count()

                })
                .OrderByDescending(r =>r.Date)
                .ToListAsync();

            return Ok(revenue);
        }
    }
}
