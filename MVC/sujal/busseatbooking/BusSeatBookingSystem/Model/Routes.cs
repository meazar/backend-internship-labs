using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BusSeatBookingSystem.Model
{
    public class Routes
    {
        public int RouteId { get; set; }

        public string FromLocation { get; set; }

        public string ToLocation { get; set; }


        public string DistanceKm { get; set; }

        public int DurationMinutes { get; set; }

        public decimal TicketPrice { get; set; }
    }
}
