using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Model
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int ScheduleId { get; set; }
        public int SeatId { get; set; }

        public DateTime BookingTime { get; set; }

        public string Status { get; set; }
        public string UserName { get; set; }

        public string FromLocation { get; set; }

        public string ToLocation { get; set; }

        public DateTime DepartureTime { get; set; }

        public string SeatNumber { get; set; }

    }
}
