using APIBusSeatBookingManagement.Database;
using APIBusSeatBookingManagement.Dto;
using APIBusSeatBookingManagement.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APIBusSeatBookingManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class BusController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BusController> _logger;

        public BusController(AppDbContext context, ILogger<BusController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BusDto>>> GetBuses()
        {
            var buses = await _context.Buses
                .OrderBy(b => b.BusNumber)
                .Select(b => new BusDto
                {
                    BusId = b.BusId,
                    BusNumber = b.BusNumber,
                    BusType = b.BusType,
                    TotalSeats = b.TotalSeats,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            return Ok(buses);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BusDto>> GetBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);

            if (bus == null)
                return NotFound(new { message = $"Bus with ID {id} not found" });

            var busDto = new BusDto
            {
                BusId = bus.BusId,
                BusNumber = bus.BusNumber,
                BusType = bus.BusType,
                TotalSeats = bus.TotalSeats,
                CreatedAt = bus.CreatedAt
            };

            return Ok(busDto);
        }

        
        [HttpPost("AddBus")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BusDto>> AddBus([FromBody]CreateBusDto createbusdto)
        {   
            try
            {
                var existingBus = await _context.Buses.FirstOrDefaultAsync(b => b.BusNumber == createbusdto.BusNumber);

                if(existingBus != null)
                {
                    return BadRequest(new { message = $"Bus number '{createbusdto.BusNumber}' already exists" });
                }
                var bus = new Bus
                {
                    BusNumber = createbusdto.BusNumber,
                    BusType = createbusdto.BusType,
                    TotalSeats = createbusdto.TotalSeats,
                    CreatedAt = DateTime.UtcNow

                };
                _context.Buses.Add(bus);
                await _context.SaveChangesAsync();


                var busDto = new BusDto
                {
                    BusId = bus.BusId,
                    BusNumber = bus.BusNumber,
                    BusType = bus.BusType,
                    TotalSeats = bus.TotalSeats,
                    CreatedAt = bus.CreatedAt
                };

                return CreatedAtAction(nameof(GetBus), new { id = bus.BusId }, busDto);              

            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            
        }


        [HttpGet("search/bus")]

        public async Task<IActionResult> SearchBuses (
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] DateTime? date)
        {
            var searchDate = date ?? DateTime.UtcNow.Date;
            var schedules = await _context.Schedule
                .Include(s => s.Bus)
                .Include(s => s.Route)
                .Include(s => s.Seats)
                .Where(s => s.Route.FromLocation.Contains(from)
                && s.Route.ToLocation.Contains(to)
                && s.DepartureTime.Date == (date ?? DateTime.UtcNow).Date
                && s.DepartureTime > DateTime.UtcNow)


                .Select(s => new SearchResultDto
                {
                    ScheduleId= s.ScheduleId,
                    BusNumber = s.Bus.BusNumber,
                    BusType = s.Bus.BusType,
                    FromLocation = s.Route.FromLocation,
                    ToLocation = s.Route.ToLocation,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime,
                    Price=  s.Price,
                    AvailableSeats = s.Seats.Count(seat => !seat.IsBooked),
                    AvailableSeatNumbers = s.Seats
                    .Where(seat => !seat.IsBooked)
                    .Select(seat => seat.SeatNumber)
                    .ToList()


                })
            .ToListAsync();

            
            return Ok(schedules);

        }
       


        [HttpGet("schedule/{id}/seats")]

        public async Task<IActionResult> GetSeatAvailable(int id)
        {
            var schedule = await _context.Schedule
                .Include(s=> s.Seats)
                .Include(s=> s.Bus)
                .FirstOrDefaultAsync(s => s.ScheduleId == id);

            if(schedule == null)
            {
                return NotFound("Schedule is not found");

            }

            var seatAvailable = schedule.Seats
                .Select(s => new
                {
                    s.SeatId,
                    s.SeatNumber,
                    s.IsBooked,
                })
            .ToList();


            return Ok(new
            {
                ScheduleId = schedule.ScheduleId,
                BusNumbers = schedule.Bus.BusNumber,
                TotalSeat = schedule.Bus.TotalSeats,
                AvailableSeats = schedule.Seats.Count(s => !s.IsBooked),
                Seats = seatAvailable
            });




        }



    }
}
