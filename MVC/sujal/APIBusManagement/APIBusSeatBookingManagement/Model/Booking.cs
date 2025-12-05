namespace APIBusSeatBookingManagement.Model
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int ScheduleId { get; set; }

        public int SeatId { get; set; }


        public DateTime BookingTime { get; set; } = DateTime.Now;
        public string Status { get; set; }


        public virtual User User { get; set; }

        public virtual Schedule Schedule { get; set; }
        public virtual Seat Seat { get; set; }
        public  virtual Payment Payment { get; set; }

  

    }
}
