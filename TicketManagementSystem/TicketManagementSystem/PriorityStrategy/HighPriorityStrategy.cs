using EmailService;
using TicketManagementSystem.Interfaces;

namespace TicketManagementSystem
{
    public class HighPriorityStrategy : IPriorityStrategy
    {
        private IEmailService _emailService;

        public HighPriorityStrategy(IEmailService emailService ) {
            _emailService = emailService;
        }

        public void ChooseNotification(string title, string assignedTo)
        {
            _emailService.SendEmailToAdministrator(title, assignedTo);
        }

        public double GetPrice()
        {
            return 100;
        }
    }
}
