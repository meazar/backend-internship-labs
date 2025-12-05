namespace APIBusSeatBookingManagement.Dto
{
    public class BusDto
    {
        public int BusId { get; set; }
        public string BusNumber { get; set; }
        public string BusType { get; set; }
        public int TotalSeats{ get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
