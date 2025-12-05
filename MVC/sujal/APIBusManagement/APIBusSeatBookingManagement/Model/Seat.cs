namespace APIBusSeatBookingManagement.Model
{
    public class Seat
    {
        public int SeatId { get; set; }

        public int ScheduleId { get; set; }

        public string SeatNumber { get; set; }

        public bool IsBooked { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
    }
}
