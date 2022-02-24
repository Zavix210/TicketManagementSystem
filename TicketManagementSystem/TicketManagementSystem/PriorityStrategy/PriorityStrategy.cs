using TicketManagementSystem.Interfaces;

namespace TicketManagementSystem
{
    public class PriorityStrategy : IPriorityStrategy
    {
        public double GetPrice() { 
            return 50; 
        }

        public void ChooseNotification(string title, string assignedTo){ }
    }
}
