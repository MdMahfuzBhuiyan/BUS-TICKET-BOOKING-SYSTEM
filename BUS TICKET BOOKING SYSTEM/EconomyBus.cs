namespace BusTicketBookingSystem
{
    public class EconomyBus : Bus
    {
        public override string CoachClass => "Economy";
        public override int TotalSeats => 36;
        public override int ColumnsPerRow => 4; 
    }
}