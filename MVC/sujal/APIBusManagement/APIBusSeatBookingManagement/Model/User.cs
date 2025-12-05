using System.ComponentModel.DataAnnotations;

namespace APIBusSeatBookingManagement.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } = "Customer";


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();


        

        


    }
}
