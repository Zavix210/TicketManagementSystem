using EmailService;
using Moq;
using NUnit.Framework;
using System;
using TicketManagementSystem;
using TicketManagementSystem.Interfaces;

namespace TicketServiceTest
{
    public class PriorityStrategyTest
    {

        PriorityFactory PriorityFactory;
        Mock<IEmailService> emailServiceMock;

        [SetUp]
        public void Setup()
        {

            emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(x => x.SendEmailToAdministrator(It.IsAny<string>(), It.IsAny<string>()));
            PriorityFactory = new PriorityFactory(emailServiceMock.Object);
        }

        [Test]
        public void MeduimShallbeInstanceOfPriorityStrategy()
        {
            Priority priority = Priority.Medium;
            Assert.IsInstanceOf(typeof(PriorityStrategy), PriorityFactory.GetPriorityStrategy(priority));
        }

        [Test]
        public void LowShallbeInstanceOfPriorityStrategy()
        {
            Priority priority = Priority.Low;

            PriorityFactory.GetPriorityStrategy(priority).ChooseNotification(It.IsAny<string>(), It.IsAny<string>());
            Assert.IsInstanceOf(typeof(PriorityStrategy), PriorityFactory.GetPriorityStrategy(priority));
        }

        [Test]
        public void ShallbeInstanceOfHighPriortyStrategy()
        {
            Priority priority = Priority.High;

            PriorityFactory.GetPriorityStrategy(priority).ChooseNotification(It.IsAny<string>(), It.IsAny<string>());
            Assert.IsInstanceOf(typeof(HighPriorityStrategy), PriorityFactory.GetPriorityStrategy(priority));
        }


        [Test]
        public void ShallReturnPrice100onHighPriorityStrategy()
        {
            double price = (new HighPriorityStrategy(emailServiceMock.Object)).GetPrice();
            Assert.That(() => price == 100.00);
        }

        [Test]
        public void ShallReturnPrice50onPriorityStrategy()
        {
            double price = (new PriorityStrategy()).GetPrice();
            Assert.That(() => price == 50.00);
        }

        [Test]
        public void ShallCallSendEmailOnHighPriorityStrategy()
        {
            (new HighPriorityStrategy(emailServiceMock.Object)).ChooseNotification(It.IsAny<string>(),It.IsAny<string>());
            emailServiceMock.Verify(x => x.SendEmailToAdministrator(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }


        [Test]
        public void ShallNotSendEmailOnPriorityStrategy()
        {
            (new PriorityStrategy()).ChooseNotification(It.IsAny<string>(), It.IsAny<string>());
            emailServiceMock.Verify(x => x.SendEmailToAdministrator(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }
    }
}
