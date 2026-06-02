using System;

namespace BusTicketBookingSystem
{
    public abstract class Bus
    {
        public int Id { get; set; }
        public string CoachNumber { get; set; }
        public abstract string CoachClass { get; }
        public abstract int TotalSeats { get; }
        public abstract int ColumnsPerRow { get; }

        public virtual bool IsValidSeatNumber(string seatNo)
        {
            if (string.IsNullOrWhiteSpace(seatNo) || seatNo.Length < 2) return false;
            
            string rowPart = seatNo.Substring(0, seatNo.Length - 1);
            char colPart = seatNo[seatNo.Length - 1];

            if (!int.TryParse(rowPart, out int row) || row <= 0) return false;

            int colIndex = colPart - 'A';
            if (colIndex < 0 || colIndex >= ColumnsPerRow) return false;

            int seatIndex = (row - 1) * ColumnsPerRow + colIndex;
            return seatIndex >= 0 && seatIndex < TotalSeats;
        }
    }
}