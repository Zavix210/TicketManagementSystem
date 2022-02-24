namespace TicketManagementSystem.Interfaces
{
    public interface IUserService
    {
        User GetUser(string username);

        User GetAccountManager();
    }
}
