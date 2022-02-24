namespace TicketManagementSystem.Interfaces
{
    public interface IPriorityFactory
    {
        IPriorityStrategy GetPriorityStrategy(Priority priority);
    }
}
