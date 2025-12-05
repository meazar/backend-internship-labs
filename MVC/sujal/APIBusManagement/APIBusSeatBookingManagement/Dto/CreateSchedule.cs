namespace APIBusSeatBookingManagement.Dto
{
    public class CreateSchedule
    {
        public int BusId { get; set; }

        public int RouteId { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Decimal Price { get; set; }
    }


    public class ScheduleSimpleDto
    {
        public int ScheduleId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public RouteSimpleDto Route { get; set; }
    }

    public class ScheduleDto : ScheduleSimpleDto
    {
        public int AvailableSeats { get; set; }
        public List<string> AvailableSeatNumbers { get; set; }
        public BusSimpleDto Bus { get; set; }
    }

}
