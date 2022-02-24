using EmailService;
using System.Collections.Generic;
using TicketManagementSystem.Interfaces;

namespace TicketManagementSystem
{
    public class PriorityFactory : IPriorityFactory
    {

        private readonly Dictionary<Priority, IPriorityStrategy> _priorityMap = new Dictionary<Priority, IPriorityStrategy>();

        public PriorityFactory() {
            _priorityMap.Add(Priority.High, new HighPriorityStrategy(new EmailServiceProxy()));
            _priorityMap.Add(Priority.Medium, new PriorityStrategy());
            _priorityMap.Add(Priority.Low, new PriorityStrategy());
        }

        public PriorityFactory(IEmailService emailService)
        {
            _priorityMap.Add(Priority.High, new HighPriorityStrategy(emailService));
            _priorityMap.Add(Priority.Medium, new PriorityStrategy());
            _priorityMap.Add(Priority.Low, new PriorityStrategy());
        }

        public IPriorityStrategy GetPriorityStrategy(Priority priority)
        {
            return _priorityMap[priority];
        }
    }
}
