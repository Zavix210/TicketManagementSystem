using Moq;
using NUnit.Framework;
using TicketManagementSystem;
using TicketManagementSystem.Interfaces;

namespace TicketServiceTest
{
    public class UserServiceTest
    {
        Mock<IUserRepository> _userRepoMock;
        IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Test]
        public void ShallThrowExceptionOnNullAssignedTo()
        {
            _userRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns<User>(null);

            string? assignedTo = null;

            Assert.That(() => _userService.GetUser(assignedTo), Throws.TypeOf<UnknownUserException>());
            _userRepoMock.Verify(x => x.GetUser(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void ShallThrowExceptionOnUnkownAssignedTo()
        {
            _userRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns<User>(null);

            string? assignedTo = "Jeremy";

            Assert.That(() => _userService.GetUser(assignedTo), Throws.TypeOf<UnknownUserException>());
            _userRepoMock.Verify(x => x.GetUser(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ShallReturnUserAssignedTo()
        {
            User returnValue = new User(){
                Username = "Jeremy",
                FirstName= "Jeremy",
                LastName = "Walker"
            };

            _userRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(returnValue);

            string? assignedTo = "Jeremy";

            User resultValue = _userService.GetUser(assignedTo);

            Assert.That(() => resultValue.Username == returnValue.Username &&
                              resultValue.FirstName == returnValue.FirstName &&
                              resultValue.LastName == returnValue.LastName);

            _userRepoMock.Verify(x => x.GetUser(It.IsAny<string>()), Times.Once);
        }

    }
}
