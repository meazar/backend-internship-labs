using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Model
{
    public class Seat
    {
        public int SeatId { get; set; }
        public int BusId { get; set; }
        public string SeatNumber { get; set; }
        public bool IsBooked { get; set; }

    }
}
