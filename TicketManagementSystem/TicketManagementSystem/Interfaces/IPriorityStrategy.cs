namespace TicketManagementSystem.Interfaces
{
    public interface IPriorityStrategy
    {
        double GetPrice();

        void ChooseNotification(string title, string assignedTo);
    }
}
