using System.ComponentModel.DataAnnotations;

namespace APIBusSeatBookingManagement.Model
{
    public class Bus
    {
        [Key]
        public int BusId { get; set; }

        public string BusNumber { get; set; }

        public string BusType { get; set; }

        public int TotalSeats { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    }
}
