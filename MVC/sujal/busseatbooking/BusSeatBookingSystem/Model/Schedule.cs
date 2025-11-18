using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Model
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public int BusId { get; set; }

        public int RouteId { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public int AvailableSeats { get; set; }

        public string BusNumber { get; set; }

        public string FromLocation { get; set; }

        public string ToLocation { get; set; }
        public decimal TicketPrice { get; set; }

    }
}
