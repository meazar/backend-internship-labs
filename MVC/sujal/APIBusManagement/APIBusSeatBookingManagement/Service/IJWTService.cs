using APIBusSeatBookingManagement.Model;

namespace APIBusSeatBookingManagement.Service
{
    public interface IJWTService
    {
         string GenerateToken(User user);
    }
}
