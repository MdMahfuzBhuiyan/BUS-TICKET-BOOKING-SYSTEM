using System.Collections.Generic;
using System.Linq;

namespace BusTicketBookingSystem
{
    public class BookingSystemRepository : IBookingRepository
    {
        private readonly List<User> _users = new List<User>();
        private readonly List<Bus> _buses = new List<Bus>();
        private readonly List<Schedule> _schedules = new List<Schedule>();
        private readonly List<Invoice> _invoices = new List<Invoice>();
        private readonly List<Ticket> _tickets = new List<Ticket>();

        public void AddUser(User user) => _users.Add(user);
        public List<User> GetAllUsers() => _users;
        public void AddBus(Bus bus) => _buses.Add(bus);
        public List<Bus> GetAllBuses() => _buses;
        public void AddSchedule(Schedule schedule) => _schedules.Add(schedule);
        public List<Schedule> GetAllSchedules() => _schedules;
        public Schedule GetScheduleById(int id) => _schedules.FirstOrDefault(s => s.Id == id);
        public User GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);
        public void AddInvoice(Invoice invoice) => _invoices.Add(invoice);
        public Invoice GetInvoiceById(int id) => _invoices.FirstOrDefault(i => i.Id == id);
        public List<Invoice> GetInvoicesByUserId(int userId) => _invoices.Where(i => i.UserId == userId).ToList();
        public void AddTicket(Ticket ticket) => _tickets.Add(ticket);
        public Ticket GetTicketById(int id) => _tickets.FirstOrDefault(t => t.Id == id);
    }
}