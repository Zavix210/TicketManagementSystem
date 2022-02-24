using EmailService;
using Moq;
using NUnit.Framework;
using System;
using TicketManagementSystem;
using TicketManagementSystem.Interfaces;

namespace TicketServiceTest
{
    public class TicketServiceTest
    {
        ITicketService _ticketService;
        Mock<ITicketRepoWrapper> TicketRepoWrapperMock;
        Mock<IUserService> userServiceMock;
        Mock<IPriorityFactory> priorityFactory;
        Mock<ICalculatePriority> calculatePriority;

        Ticket? result;

        [SetUp]
        public void Setup()
        {
            TicketRepoWrapperMock = new Mock<ITicketRepoWrapper>();
            userServiceMock = new Mock<IUserService>();
            calculatePriority = new Mock<ICalculatePriority>();
            priorityFactory = new Mock<IPriorityFactory>();

            TicketRepoWrapperMock.Setup(x => x.CreateTicket(It.IsAny<Ticket>())).Callback<Ticket>(r => result = r);
            TicketRepoWrapperMock.Setup(x => x.UpdateTicket(It.IsAny<Ticket>())).Callback<Ticket>(r => result = r);

            userServiceMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new User());

            priorityFactory.Setup(x => x.GetPriorityStrategy(It.IsAny<Priority>())).Returns(new PriorityStrategy());

            _ticketService = new TicketService(userServiceMock.Object, TicketRepoWrapperMock.Object, calculatePriority.Object,
                priorityFactory.Object);
        }

        [Test]
        public void ShallThrowExceptionOnNullTitle()
        {
            string? title = null;
            string? description = "The system crashed when user performed a search";

            Assert.That(() => _ticketService.CreateTicket(title, It.IsAny<Priority>(), description, It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Throws.TypeOf<InvalidTicketException>());
        }

        [Test]
        public void ShallThrowExceptionOnNullDescription()
        {
            string? title = "System Crash";
            string? description = null;

            Assert.That(() => _ticketService.CreateTicket(title, It.IsAny<Priority>(), It.IsAny<string>(), description, It.IsAny<DateTime>(), It.IsAny<bool>()), Throws.TypeOf<InvalidTicketException>());
        }

        [Test]
        public void ShallThrowExceptionOnEmptyTitle()
        {
            string? title = string.Empty;
            string? description = "The system crashed when user performed a search";

            Assert.That(() => _ticketService.CreateTicket(title, It.IsAny<Priority>(), It.IsAny<string>(), description, It.IsAny<DateTime>(), It.IsAny<bool>()), Throws.TypeOf<InvalidTicketException>());
        }

        [Test]
        public void ShallThrowExceptionOnEmptyDescription()
        {
            string? title = "System Crash";
            string? description = string.Empty;

            Assert.That(() => _ticketService.CreateTicket(title, It.IsAny<Priority>(), It.IsAny<string>(), description, It.IsAny<DateTime>(), It.IsAny<bool>()), Throws.TypeOf<InvalidTicketException>());
        }

        [Test]
        public void ShallCreateNormalTicket()
        {
            var x = new User() { FirstName = "A", LastName = "W", Username = "AW" };
            userServiceMock.Setup(x => x.GetAccountManager()).Returns(x);

            DateTime dateTime = DateTime.Now;
            string desc = "I have an issue";
            string title = "Help issue";
            Priority prio = Priority.High;
            calculatePriority.Setup(x => x.Calculate(It.IsAny<Priority>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(prio);

            var ticket = new Ticket()
            {
                AssignedUser = x,
                Created = dateTime,
                Description = desc,
                Title = title,
                Priority = prio
            };

            var resultTicket = _ticketService.CreateTicket(ticket.Title, ticket.Priority, ticket.AssignedUser.Username, ticket.Description, ticket.Created, false);

            Assert.That(() => result?.Title == ticket.Title
             && result?.Priority == ticket.Priority
             && result?.Created == dateTime
             && result?.Description == desc
             && result.PriceDollars == ticket.PriceDollars
            );
            Assert.That(() => result?.PriceDollars == 0);
            Assert.That(() => result?.AccountManager == null);
        }

        [Test]
        public void ShallCreatePricedTicket()
        {
            var accountmanager = new User() { FirstName = "A", LastName = "W", Username = "AW" };
            userServiceMock.Setup(x => x.GetAccountManager()).Returns(accountmanager);
            var user = new User() { FirstName = "B", LastName = "Z", Username = "BZ" };
            userServiceMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(user);

            DateTime dateTime = DateTime.Now;
            string desc = "I have an issue";
            string title = "Help issue";
            Priority prio = Priority.High;
            calculatePriority.Setup(x => x.Calculate(It.IsAny<Priority>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns(prio);

            var ticket = new Ticket()
            {
                PriceDollars = 50,
                AccountManager = accountmanager,
                AssignedUser = user,
                Created = dateTime,
                Description = desc,
                Title = title,
                Priority = prio
            };

            _ticketService.CreateTicket(ticket.Title, ticket.Priority, ticket.AssignedUser.Username, ticket.Description, ticket.Created, true);

            Assert.That(() => result?.Title == ticket.Title 
                        && result?.Priority == ticket.Priority
                        && result?.Created == dateTime
                        && result?.Description == desc
                        && result.PriceDollars == ticket.PriceDollars
                       );

            Assert.That(() => result?.AccountManager.FirstName == accountmanager.FirstName
                            && result?.AccountManager.LastName == accountmanager.LastName
                            && result?.AccountManager.Username == accountmanager.Username
                             );

            Assert.That(() => result?.AssignedUser.FirstName == user.FirstName
                && result?.AssignedUser.LastName == user.LastName
                && result?.AssignedUser.Username == user.Username
                 );
        }

        [Test]
        public void ShallThrowExceptionOnNoTicket()
        {
            string? assigned = "jack";
            int id = 1;

            TicketRepoWrapperMock.Setup(x => x.GetTicket(id)).Returns<Ticket>(null);

            Assert.That(() => _ticketService.AssignTicket(id, assigned),  Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public void ShallAssignTicketIfTicketExists()
        {
            User assigned = new User() { FirstName = "Jack", LastName = "Walker", Username = "JJW" };
            int id = 1;

            TicketRepoWrapperMock.Setup(x => x.GetTicket(id)).Returns(new Ticket());
            userServiceMock.Setup(x => x.GetUser(assigned.FirstName)).Returns(assigned);

            _ticketService.AssignTicket(id, assigned.FirstName);
            Assert.That(() => result?.AssignedUser.FirstName == assigned.FirstName);
        }
    }
}