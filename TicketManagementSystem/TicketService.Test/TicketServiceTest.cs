using Moq;
using NUnit.Framework;
using System;
using TicketManagementSystem;

namespace TicketServiceTest
{
    public class TicketServiceTest
    {
        ITicketService _ticketService; 

        [SetUp]
        public void Setup()
        {
            _ticketService = new TicketService();
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

    }
}