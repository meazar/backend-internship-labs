using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APIBusSeatBookingManagement.Model
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        public int BusId { get; set; }

        public int RouteId { get; set; }


        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

        public virtual Bus Bus { get; set; }
        public virtual Routes Route { get; set; }
        public  virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
 