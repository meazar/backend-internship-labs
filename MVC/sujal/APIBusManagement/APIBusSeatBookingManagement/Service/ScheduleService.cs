using APIBusSeatBookingManagement.Database;
using APIBusSeatBookingManagement.Dto;
using APIBusSeatBookingManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace APIBusSeatBookingManagement.Service
{
    public class ScheduleService :IScheduleService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ScheduleService> _logger;



        public ScheduleService(AppDbContext context ,ILogger<ScheduleService> logger)
        {
            _context = context;
            _logger = logger;

        }


        public async Task<Schedule> CreateScheduleAsync(CreateSchedule scheduleDto)
        {
            if(!await ValidateScheduleAsync(scheduleDto))
            {
                throw new ArgumentException("Schedule Validation Failed");

            }

            var schedule = new Schedule
            {
                BusId = scheduleDto.BusId,
                RouteId = scheduleDto.RouteId,
                DepartureTime = scheduleDto.DepartureTime,
                ArrivalTime = scheduleDto.ArrivalTime,
                Price = scheduleDto.Price
            };

            _context.Schedule.Add(schedule);
            await _context.SaveChangesAsync();

            return schedule;
        }


        public async Task<bool> ValidateScheduleAsync(CreateSchedule scheduleDto)
        {
            // Check if departure time is in the future
            if (scheduleDto.DepartureTime <= DateTime.UtcNow)
                return false;

            // Check if arrival time is after departure
            if (scheduleDto.ArrivalTime <= scheduleDto.DepartureTime)
                return false;

            // Check if bus exists and is active
            var bus = await _context.Buses.FindAsync(scheduleDto.BusId);
            if (bus == null)
                return false;

            // Check if route exists
            var route = await _context.Routes.FindAsync(scheduleDto.RouteId);
            if (route == null)
                return false;

            // Check for schedule conflicts (same bus at overlapping times)
            var conflictingSchedules = await _context.Schedule
                .Where(s => s.BusId == scheduleDto.BusId)
                .Where(s => (s.DepartureTime <= scheduleDto.ArrivalTime &&
                           s.ArrivalTime >= scheduleDto.DepartureTime))
                .AnyAsync();

            return !conflictingSchedules;
        }


        public async Task GenerateSeatsForScheduleAsync(int scheduleId, int totalSeats)
        {
            var seats = new List<Seat>();
            var rows = (int)Math.Ceiling(totalSeats / 4.0);


            for (int row = 0; row < row; row++)
            {
                for(int col = 1; col<= 4 && seats.Count < totalSeats; col++)
                {
                    var rowChar = (char)('A' + row);
                    seats.Add(new Seat
                    {
                        ScheduleId = scheduleId,
                        SeatNumber = $"{rowChar}{col}",
                        IsBooked = false
                    });

                }
            }
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Generated {seats.Count} seats for schedule {scheduleId}");
        }
    }
}
