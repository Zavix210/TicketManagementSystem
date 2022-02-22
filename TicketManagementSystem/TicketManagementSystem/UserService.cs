﻿using TicketManagementSystem.Interfaces;
namespace TicketManagementSystem
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetAccountManager(bool isPayingCustomer)
        {
            // Only paid customers have an account manager.
            return isPayingCustomer ? _userRepository.GetAccountManager() : null;
        }

        public User GetUser(string assignedTo)
        {
            User user = null;
            if (assignedTo != null)
                user = _userRepository.GetUser(assignedTo);

            if (user == null)
                throw new UnknownUserException("User " + assignedTo + " not found");

            return user;
        }
    }
}
