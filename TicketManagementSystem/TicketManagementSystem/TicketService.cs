using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using EmailService;
using TicketManagementSystem.Interfaces;

namespace TicketManagementSystem
{
    public class TicketService : ITicketService
    {
        private readonly IUserService _userService;
        private readonly ITicketRepoWrapper _ticketRepoWrapper;
        private readonly ICalculatePriority _calculatePriority;
        private readonly IPriorityFactory _priorityFactory;

        public TicketService()
        {
            _userService = new UserService();
            _ticketRepoWrapper = new TicketRepoWrapper();
            _calculatePriority = new CalculatePriority();
            _priorityFactory = new PriorityFactory();
        }

        public TicketService(IUserService userService, ITicketRepoWrapper ticketRepoWrapper,
            ICalculatePriority emailService, IPriorityFactory priorityFactory)
        {
            _userService = userService;
            _ticketRepoWrapper = ticketRepoWrapper;
            _calculatePriority = emailService;
            _priorityFactory = priorityFactory;
        }


        public int CreateTicket(string t, Priority p, string assignedTo, string desc, DateTime d, bool isPayingCustomer)
        {
            // Check if t or desc are null or Empty and throw exception
            if (string.IsNullOrEmpty(t) || string.IsNullOrEmpty(desc))
                throw new InvalidTicketException("Title or description were null");

            User user = _userService.GetUser(assignedTo);

            p = _calculatePriority.Calculate(p,d,t);

            var prioritystrategy = _priorityFactory.GetPriorityStrategy(p);

            prioritystrategy.ChooseNotification(t, assignedTo);

            Ticket ticket = new Ticket()
            {
                Title = t,
                AssignedUser = user,
                Priority = p,
                Description = desc,
                Created = d
            };

            if (isPayingCustomer){
                // Only paid customers have an account manager.
                ticket.AccountManager = _userService.GetAccountManager();
                ticket.PriceDollars = prioritystrategy.GetPrice();
            }

            var id = _ticketRepoWrapper.CreateTicket(ticket);

            // Return the id
            return id;
        }


        public void AssignTicket(int id, string username)
        {
            User user = _userService.GetUser(username);

            var ticket = _ticketRepoWrapper.GetTicket(id);

            if (ticket == null)
                throw new ApplicationException("No ticket found for id " + id);

            ticket.AssignedUser = user;

            _ticketRepoWrapper.UpdateTicket(ticket);
        }

        // would remove unsure if breaks limitation 3.
        private void WriteTicketToFile(Ticket ticket)
        {
            var ticketJson = JsonSerializer.Serialize(ticket);
            File.WriteAllText(Path.Combine(Path.GetTempPath(), $"ticket_{ticket.Id}.json"), ticketJson);
        }
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
