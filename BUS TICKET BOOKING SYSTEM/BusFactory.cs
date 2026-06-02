using System;

namespace BusTicketBookingSystem
{
    public static class BusFactory
    {
        public static Bus CreateBus(string coachClass)
        {
            if (string.Equals(coachClass, "Business", StringComparison.OrdinalIgnoreCase))
                return new BusinessBus();
            if (string.Equals(coachClass, "Economy", StringComparison.OrdinalIgnoreCase))
                return new EconomyBus();
            return null;
        }
    }
}