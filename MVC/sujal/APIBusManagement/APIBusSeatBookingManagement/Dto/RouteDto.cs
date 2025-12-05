namespace APIBusSeatBookingManagement.Dto
{
    public class RouteDto
    {
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public decimal DistanceKm   { get; set; }
        public int DurationsMinutes { get; set; }
    }
    public class RouteSimpleDto
    {
        public int RouteId { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public double DistanceKm { get; set; }
        public int DurationMinutes { get; set; }
    }
}

public class BusSimpleDto
{
    public int BusId { get; set; }
    public string BusNumber { get; set; }
    public string BusType { get; set; }
}

