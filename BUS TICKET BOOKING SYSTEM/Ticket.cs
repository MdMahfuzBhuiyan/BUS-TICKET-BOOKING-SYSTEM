namespace BusTicketBookingSystem
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public string SeatNumber { get; set; }
        public bool IsPaid { get; set; } = false;
    }
}