namespace APIBusSeatBookingManagement.Dto
{
    public class SearchResultDto
    {
        public int ScheduleId { get; set; }
        public string BusNumber { get; set; }
        public string BusType { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public List<string> AvailableSeatNumbers { get; set; }
    }
}
