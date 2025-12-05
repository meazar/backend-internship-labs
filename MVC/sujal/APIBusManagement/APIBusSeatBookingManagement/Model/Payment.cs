using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APIBusSeatBookingManagement.Model
{
    public class Payment
    {

        [Key]
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public DateTime paymentTime { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
