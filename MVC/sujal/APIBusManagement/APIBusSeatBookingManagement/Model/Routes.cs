using System.ComponentModel.DataAnnotations;

namespace APIBusSeatBookingManagement.Model
{
    public class Routes
    {
        [Key]
        public int RouteId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public double DistanceKm { get; set; }

        public int DurationMinutes { get; set; }


        //public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
