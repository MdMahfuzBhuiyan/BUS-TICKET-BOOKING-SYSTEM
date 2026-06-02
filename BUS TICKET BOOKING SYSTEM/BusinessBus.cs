namespace BusTicketBookingSystem
{
    public class BusinessBus : Bus
    {
        public override string CoachClass => "Business";
        public override int TotalSeats => 27;
        public override int ColumnsPerRow => 3; 
    }
}