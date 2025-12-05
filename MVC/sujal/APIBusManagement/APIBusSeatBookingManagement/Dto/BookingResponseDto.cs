namespace APIBusSeatBookingManagement.Dto
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public string BusNumber { get; set; }

        public string FromLocation { get; set; }

        public string ToLocation { get; set; }
        public DateTime DepartureTime { get; set; }

        public string SeatNumber { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public DateTime BookingTime {  get; set; }
    }
}
