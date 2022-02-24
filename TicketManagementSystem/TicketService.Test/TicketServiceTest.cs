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
            priorityFactory.Setup(x => x.GetPriorityStrategy(Priority.High)).Returns(new PriorityStrategy());
            priorityFactory.Setup(x => x.GetPriorityStrategy(Priority.Medium)).Returns(new PriorityStrategy());
            priorityFactory.Setup(x => x.GetPriorityStrategy(Priority.Low)).Returns(new PriorityStrategy());

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