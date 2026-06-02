using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BusTicketBookingSystem
{
    class Program
    {
        private static readonly IBookingRepository _repo = new BookingSystemRepository();
        
        private static int _userCounter = 1;
        private static int _busCounter = 1;
        private static int _scheduleCounter = 1;
        private static int _ticketCounter = 1;
        private static int _invoiceCounter = 1;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- BUS TICKET BOOKING SYSTEM ---");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Show Users");
                Console.WriteLine("3. Create Bus");
                Console.WriteLine("4. Show Buses");
                Console.WriteLine("5. Create schedule");
                Console.WriteLine("6. Show schedules");
                Console.WriteLine("7. Show schedule details");
                Console.WriteLine("8. Book ticket");
                Console.WriteLine("9. Show Invoices of an user");
                Console.WriteLine("10. Pay Invoice");
                Console.WriteLine("11. Show Tickets of an user");
                Console.WriteLine("12. Exit");
                Console.Write("\nEnter an option : ");

                if (!int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("\nInvalid option selection! Press Enter to continue...");
                    Console.ReadLine();
                    continue;
                }

                switch (option)
                {
                    case 1: CreateUser(); break;
                    case 2: ShowUsers(); break;
                    case 3: CreateBus(); break;
                    case 4: ShowBuses(); break;
                    case 5: CreateSchedule(); break;
                    case 6: ShowSchedules(); break;
                    case 7: ShowScheduleDetails(); break;
                    case 8: BookTicket(); break;
                    case 9: ShowUserInvoices(); break;
                    case 10: PayInvoice(); break;
                    case 11: ShowUserTickets(); break;
                    case 12: return;
                    default: 
                        Console.WriteLine("\nOption out of bounds! Press Enter to continue..."); 
                        Console.ReadLine();
                        break;
                }
            }
        }

       private static void CreateUser()
{
    Console.Clear();
    Console.WriteLine("--- Creating User ---");
    Console.Write("Enter Full Name: ");
    string name = Console.ReadLine();
    Console.Write("Enter Mobile Number: ");
    string mobile = Console.ReadLine()?.Trim();
    Console.Write("Enter Email Address: ");
    string email = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(mobile) || mobile.Length != 11 || !mobile.StartsWith("01") || !mobile.All(char.IsDigit))
    {
        Console.WriteLine("\nError: Invalid Mobile Number! It must be exactly 11 digits and start with '01'.");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
        return;
    }

    if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains(".") || email.IndexOf('@') > email.LastIndexOf('.'))
    {
        Console.WriteLine("\nError: Invalid Email Format! Please provide a proper email (e.g., example@gmail.com).");
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
        return;
    }

    var existingUsers = _repo.GetAllUsers();
    if (existingUsers != null)
    {
        foreach (var u in existingUsers)
        {
            if ((u.PhoneNumber != null && u.PhoneNumber == mobile) || 
                (u.Email != null && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("\nError: A user with this mobile number or email already exists!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }
        }
    }

    User user = new User { Id = _userCounter++, Name = name, PhoneNumber = mobile, Email = email };
    _repo.AddUser(user);
    Console.WriteLine($"\nUser created successfully with ID: {user.Id}");
    Console.WriteLine("\nPress Enter to continue...");
    Console.ReadLine();
}
        private static void ShowUsers()
        {
            Console.Clear();
            Console.WriteLine("--- Show Users Output ---");
            var users = _repo.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                Console.WriteLine("User isn't created");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("\n--- User's ---");
            foreach (var u in users)
            {
                Console.WriteLine($"{u.Id} | {u.Name ?? "N/A"} | {u.PhoneNumber ?? "N/A"} | {u.Email ?? "N/A"}");
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void CreateBus()
        {
            Console.Clear();
            Console.WriteLine("--- Creating Bus ---");
            Console.Write("Enter Coach Number (e.g., 128, 256): ");
            string coachNum = Console.ReadLine()?.Trim();
            
            if (_repo.GetAllBuses().Any(b => b.CoachNumber != null && b.CoachNumber.Equals(coachNum, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("\nError: A bus with this coach number already exists!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Coach Class (Business / Economy): ");
            string coachClass = Console.ReadLine()?.Trim();

            Bus bus = BusFactory.CreateBus(coachClass);
            if (bus == null)
            {
                Console.WriteLine("\nInvalid Class Type! Bus creation failed.");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            bus.Id = _busCounter++;
            bus.CoachNumber = coachNum;
            _repo.AddBus(bus);
            Console.WriteLine($"\nBus created successfully! Bus ID: {bus.Id}, Total Seats: {bus.TotalSeats}");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void ShowBuses()
        {
            Console.Clear();
            Console.WriteLine("--- Show Buses Output ---");
            var buses = _repo.GetAllBuses();
            if (buses == null || buses.Count == 0)
            {
                Console.WriteLine("Bus isn't created");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("\n--- Buses ---");
            foreach (var b in buses)
            {
                Console.WriteLine($"{b.Id} | Coach Number {b.CoachNumber} | {b.CoachClass} class | Total Seats: {b.TotalSeats}");
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void CreateSchedule()
        {
            Console.Clear();
            Console.WriteLine("--- Creating Schedule ---");
            if (_repo.GetAllBuses().Count == 0)
            {
                Console.WriteLine("\nCannot create schedule. No bus available!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Bus ID: ");
            if (!int.TryParse(Console.ReadLine(), out int busId)) return;
            Bus bus = _repo.GetAllBuses().FirstOrDefault(b => b.Id == busId);
            if (bus == null)
            {
                Console.WriteLine("\nBus not found!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Departure City: ");
            string from = Console.ReadLine();
            Console.Write("Enter Arrival City: ");
            string to = Console.ReadLine();

            Console.Write("Enter Date (YYYY-MM-DD): ");
            string date = Console.ReadLine()?.Trim();
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nError: Invalid Date format! Must be YYYY-MM-DD (e.g., 2026-06-02).");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Time (HH:MM): ");
            string time = Console.ReadLine()?.Trim();
            if (!DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nError: Invalid Time format! Must be HH:MM in 24-hour system (e.g., 07:02 or 22:30).");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            if (_repo.GetAllSchedules().Any(s => 
                s.AssignedBus.Id == busId && 
                s.DepartureDate == date && 
                s.DepartureTime == time))
            {
                Console.WriteLine("\nError: This bus is already assigned to another schedule at the exact same date and time!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter Ticket Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)) return;

            Schedule sched = new Schedule
            {
                Id = _scheduleCounter++,
                AssignedBus = bus,
                DepartureCity = from,
                ArrivalCity = to,
                DepartureDate = date,
                DepartureTime = time,
                TicketPrice = price
            };
            _repo.AddSchedule(sched);
            Console.WriteLine("\nSchedule created successfully!");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void ShowSchedules()
        {
            Console.Clear();
            Console.WriteLine("--- Show Schedules Output ---");
            var schedules = _repo.GetAllSchedules();
            if (schedules == null || schedules.Count == 0)
            {
                Console.WriteLine("Schedule isn't created");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("\n--- Schedules ---");
            foreach (var s in schedules)
            {
                Console.WriteLine($"{s.Id}. Bus ID:{s.AssignedBus.Id:D2} | {s.DepartureCity} -> {s.ArrivalCity} | Date: {s.DepartureDate} | Time: {s.DepartureTime} | Taka: {s.TicketPrice}");
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void ShowScheduleDetails()
        {
            Console.Clear();
            Console.Write("Enter schedule ID to view details: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Schedule s = _repo.GetScheduleById(id);
            if (s == null)
            {
                Console.WriteLine("Schedule not found!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            int availableCount = s.AssignedBus.TotalSeats - s.ReservedSeats.Count;

            Console.WriteLine("\n--- Schedule Details ---");
            Console.WriteLine($"Schedule ID : {s.Id}");
            Console.WriteLine($"Bus ID : {s.AssignedBus.Id} | Coach Number : {s.AssignedBus.CoachNumber} | Type : {s.AssignedBus.CoachClass}");
            Console.WriteLine($"From :- {s.DepartureCity} -> {s.ArrivalCity}");
            Console.WriteLine($"Departure time : {s.DepartureDate} {s.DepartureTime}");
            Console.WriteLine($"Taka : {s.TicketPrice}");
            Console.WriteLine($"Total seats : {s.AssignedBus.TotalSeats}");
            Console.WriteLine($"Total seats available : {availableCount}");
            Console.WriteLine("Seat layout (X = booked, [ ] = available) :");

            int cols = s.AssignedBus.ColumnsPerRow;

            int totalSeats = s.AssignedBus.TotalSeats;
            int totalRows = (int)Math.Ceiling((double)totalSeats / cols);

            for (int r = 1; r <= totalRows; r++)
            {
                string rowOutput = "";
                for (int c = 0; c < cols; c++)
                {
                    int seatIndex = (r - 1) * cols + c;
                    if (seatIndex >= totalSeats) break;

                    char colLetter = (char)('A' + c);
                    string seatName = $"{r}{colLetter}";

                    if (s.ReservedSeats.Contains(seatName))
                    {
                        rowOutput += $"[X : {seatName}] ";
                    }
                    else
                    {
                        rowOutput += $"[ : {seatName}] ";
                    }
                }
                Console.WriteLine($"{rowOutput}Row {r}");
            }
        
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void BookTicket()
        {
            Console.Clear();
            Console.WriteLine("--- Ticket Booking ---");
            Console.Write("Enter user ID : ");
            if (!int.TryParse(Console.ReadLine(), out int userId)) return;
            User user = _repo.GetUserById(userId);
            if (user == null)
            {
                Console.WriteLine("User does not exist!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter schedule ID : ");
            if (!int.TryParse(Console.ReadLine(), out int schedId)) return;
            Schedule s = _repo.GetScheduleById(schedId);
            if (s == null)
            {
                Console.WriteLine("Schedule does not exist!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.Write("Enter seat Number (e.g., 1A, 3C) : ");
            string seatNo = Console.ReadLine()?.Trim().ToUpper();

            if (!s.AssignedBus.IsValidSeatNumber(seatNo))
            {
                Console.WriteLine("Invalid Seat Number!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            if (s.BookedSeatsHold.Contains(seatNo) || s.ReservedSeats.Contains(seatNo))
            {
                Console.WriteLine("Seat is already reserved or processing for hold!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            s.BookedSeatsHold.Add(seatNo);

            Ticket ticket = new Ticket
            {
                Id = _ticketCounter++,
                ScheduleId = s.Id,
                UserId = user.Id,
                SeatNumber = seatNo,
                IsPaid = false
            };
            _repo.AddTicket(ticket);
            user.Tickets.Add(ticket);

            Invoice invoice = new Invoice
            {
                Id = _invoiceCounter++,
                TicketId = ticket.Id,
                UserId = user.Id,
                Amount = s.TicketPrice,
                GenerationDate = DateTime.Now.ToString("2026-MM-dd"),
                IsPaid = false
            };
            _repo.AddInvoice(invoice);

            Console.WriteLine("\n--Ticket Booked Successfully ! ---");
            Console.WriteLine($"Ticket ID : {ticket.Id} | Seat : {ticket.SeatNumber}");
            Console.WriteLine($"Invoice ID : {invoice.Id} | Amount : {invoice.Amount}");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void ShowUserInvoices()
        {
            Console.Clear();
            Console.Write("Enter user ID : ");
            if (!int.TryParse(Console.ReadLine(), out int userId)) return;

            var invoices = _repo.GetInvoicesByUserId(userId);
            if (invoices.Count == 0)
            {
                Console.WriteLine("No invoices found for this user.");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\n--- Invoices ---");
            foreach (var inv in invoices)
            {
                Console.WriteLine($"Invoice ID : {inv.Id} | Ticket ID : {inv.TicketId}");
                Console.WriteLine($"Amount : {inv.Amount} | Date : {inv.GenerationDate}");
                Console.WriteLine($"Paid : {(inv.IsPaid ? "Yes" : "No")}");
                Console.WriteLine("-----------------------------------");
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void PayInvoice()
        {
            Console.Clear();
            Console.Write("Enter Invoice ID to pay : ");
            if (!int.TryParse(Console.ReadLine(), out int invId)) return;

            Invoice inv = _repo.GetInvoiceById(invId);
            if (inv == null)
            {
                Console.WriteLine("Invoice not found!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            if (inv.IsPaid)
            {
                Console.WriteLine("Invoice is already paid!");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Ticket ticket = _repo.GetTicketById(inv.TicketId);
            Schedule s = _repo.GetScheduleById(ticket.ScheduleId);

            inv.IsPaid = true;
            ticket.IsPaid = true;
            s.ReservedSeats.Add(ticket.SeatNumber);

            Console.WriteLine("Invoice paid successfully!");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        private static void ShowUserTickets()
        {
            Console.Clear();
            Console.Write("Enter user ID : ");
            if (!int.TryParse(Console.ReadLine(), out int userId)) return;

            User user = _repo.GetUserById(userId);
            if (user == null || user.Tickets.Count == 0)
            {
                Console.WriteLine("No tickets found for this user.");
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\n--- Tickets ---");
            foreach (var t in user.Tickets)
            {
                Schedule s = _repo.GetScheduleById(t.ScheduleId);
                Console.WriteLine($"Ticket ID : {t.Id} | Schedule ID : {t.ScheduleId} | Route: {s.DepartureCity} -> {s.ArrivalCity} | Seat : {t.SeatNumber} | Paid : {(t.IsPaid ? "Yes" : "No")}");
            }
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}