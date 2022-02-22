using Moq;
using NUnit.Framework;
using System;
using TicketManagementSystem;

namespace TicketServiceTest
{
    public class TicketServiceTest
    {
        ITicketService? _ticketService; 

        [SetUp]
        public void Setup()
        {
            _ticketService = new TicketService();
        }

        [Test]
        public void ShallThrowExceptionOnNullTitle()
        {
            var service = new TicketService();

            string? title = null;
            string? description = "The system crashed when user performed a search";

            Assert.That(() => service.CreateTicket(title, It.IsAny<Priority>(), description, It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Throws.TypeOf<InvalidTicketException>());
        }

        [Test]
        public void ShallThrowExceptionOnNullDescription()
        {
            var service = new TicketService();

            string? title = "System Crash";
            string? description = null;

            Assert.That(() => service.CreateTicket(title, It.IsAny<Priority>(), It.IsAny<string>(), description, It.IsAny<DateTime>(), It.IsAny<bool>()), Throws.TypeOf<InvalidTicketException>());
        }


    }
}