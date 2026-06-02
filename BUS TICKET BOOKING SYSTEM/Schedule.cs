using System.Collections.Generic;

namespace BusTicketBookingSystem
{
    public class Schedule
    {
        public int Id { get; set; }
        public Bus AssignedBus { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public decimal TicketPrice { get; set; }
        public HashSet<string> ReservedSeats { get; set; } = new HashSet<string>();
        public HashSet<string> BookedSeatsHold { get; set; } = new HashSet<string>();
    }
}