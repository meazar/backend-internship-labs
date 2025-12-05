using APIBusSeatBookingManagement.Dto;
using APIBusSeatBookingManagement.Model;
using AutoMapper;

namespace APIBusSeatBookingManagement.MiddleWare
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, UserDto>()
               .ForMember(dest => dest.TotalBookings,
                   opt => opt.MapFrom(src => src.Bookings.Count));

            CreateMap<Bus, BusDto>().ReverseMap();
            CreateMap<Route, RouteDto>().ReverseMap();

            CreateMap<CreateSchedule, Schedule>()
                .ForMember(dest => dest.DepartureTime,
                    opt => opt.MapFrom(src => src.DepartureTime.ToUniversalTime()))
                .ForMember(dest => dest.ArrivalTime,
                    opt => opt.MapFrom(src => src.ArrivalTime.ToUniversalTime()));

            CreateMap<Booking, BookingResponseDto>()
                .ForMember(dest => dest.BusNumber,
                    opt => opt.MapFrom(src => src.Schedule.Bus.BusNumber))
                .ForMember(dest => dest.FromLocation,
                    opt => opt.MapFrom(src => src.Schedule.Route.FromLocation))
                .ForMember(dest => dest.ToLocation,
                    opt => opt.MapFrom(src => src.Schedule.Route.ToLocation))
                .ForMember(dest => dest.DepartureTime,
                    opt => opt.MapFrom(src => src.Schedule.DepartureTime))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Schedule.Price))
                .ForMember(dest => dest.SeatNumber,
                    opt => opt.MapFrom(src => src.Seat.SeatNumber));

            CreateMap<Payment, paymentDto>().ReverseMap();
        }
    }
}
