namespace APIBusSeatBookingManagement.Dto
{
    public class paymentDto
    {
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
