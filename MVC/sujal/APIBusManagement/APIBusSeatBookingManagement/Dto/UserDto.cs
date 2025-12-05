namespace APIBusSeatBookingManagement.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalBookings { get; set; }
    }
}
