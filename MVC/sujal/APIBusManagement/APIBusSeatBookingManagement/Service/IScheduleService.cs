using APIBusSeatBookingManagement.Dto;
using APIBusSeatBookingManagement.Model;

namespace APIBusSeatBookingManagement.Service
{
    public interface IScheduleService
    {
        Task<Schedule> CreateScheduleAsync(CreateSchedule scheduleDto);
        Task<bool> ValidateScheduleAsync(CreateSchedule scheduleDto);

        Task GenerateSeatsForScheduleAsync(int scheduleId, int totalSeats);

    }
}
