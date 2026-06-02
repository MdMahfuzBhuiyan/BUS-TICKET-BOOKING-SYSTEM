namespace BusTicketBookingSystem
{
    public class Invoice
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string GenerationDate { get; set; }
        public bool IsPaid { get; set; } = false;
    }
}