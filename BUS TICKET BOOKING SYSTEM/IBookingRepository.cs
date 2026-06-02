using System.Collections.Generic;

namespace BusTicketBookingSystem
{
    public interface IBookingRepository
    {
        void AddUser(User user);
        List<User> GetAllUsers();
        void AddBus(Bus bus);
        List<Bus> GetAllBuses();
        void AddSchedule(Schedule schedule);
        List<Schedule> GetAllSchedules();
        Schedule GetScheduleById(int id);
        User GetUserById(int id);
        void AddInvoice(Invoice invoice);
        Invoice GetInvoiceById(int id);
        List<Invoice> GetInvoicesByUserId(int userId);
        void AddTicket(Ticket ticket);
        Ticket GetTicketById(int id);
    }
}