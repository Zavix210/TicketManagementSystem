namespace TicketManagementSystem.Interfaces
{
    // IDisposable should potentially be catered for via an instance creator
    // however as connection string is being disposed and catered for have
    // chossen to leave it off the interface as its not required
    public interface IUserRepository
    {
        User GetUser(string username);

        User GetAccountManager();
    }
}
