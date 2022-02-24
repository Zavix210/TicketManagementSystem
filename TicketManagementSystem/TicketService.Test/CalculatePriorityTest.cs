using Moq;
using NUnit.Framework;
using System;
using TicketManagementSystem;
using TicketManagementSystem.Interfaces;

namespace TicketServiceTest
{
    public class CalculatePriorityTest
    {

        CalculatePriority _calculatePriority;

        [SetUp]
        public void Setup()
        {
            _calculatePriority = new CalculatePriority();
        }

        [Test]
        public void ShallReturnTrueOverAnHour()
        {
            DateTime datecreated = DateTime.UtcNow - TimeSpan.FromMinutes(60);

            Assert.That(() => _calculatePriority.UpgradeBasedOnTime(datecreated));
        }

        [Test]
        public void ShallReturnFalseUnderAnHour()
        {
            DateTime datecreated = DateTime.UtcNow - TimeSpan.FromMinutes(59);

            Assert.That(() => !_calculatePriority.UpgradeBasedOnTime(datecreated));
        }

        [Test]
        public void ShallReturnTrueOnKeywordImportant()
        {
            string? input = "Important";

            Assert.That(() => _calculatePriority.UpgradeBasedonContent(input));
        }

        [Test]
        public void ShallReturnTrueOnKeywordCrash()
        {
            string? input = "Crash";

            Assert.That(() => _calculatePriority.UpgradeBasedonContent(input));
        }

        [Test]
        public void ShallReturnTrueOnKeywordFailure()
        {
            string? input = "Failure";

            Assert.That(() => _calculatePriority.UpgradeBasedonContent(input));
        }

        [Test]
        public void ShallReturnFalseOnNonKeyword()
        {
            string? input = "";

            Assert.That(() => _calculatePriority.UpgradeBasedonContent(input));
        }

        [Test]
        public void ShallIncreasePriorityToHigh()
        {
            Assert.That(() => _calculatePriority.IncreasePriority(Priority.Medium) == Priority.High);
        }

        [Test]
        public void ShallIncreasePriorityToMeduim()
        {
            Assert.That(() => _calculatePriority.IncreasePriority(Priority.Low) == Priority.Medium);
        }

        [Test]
        public void ShallNotIncreasePriority()
        {
            Assert.That(() => _calculatePriority.IncreasePriority(Priority.High) == Priority.High);
        }


        [Test]
        public void ShallIncreasePriorityBothConditions()
        {
            string? input = "Failure";
            DateTime datecreated = DateTime.UtcNow - TimeSpan.FromMinutes(60);

            Assert.That(() => _calculatePriority.Calculate(Priority.Medium, datecreated, input) == Priority.High);
        }

        [Test]
        public void ShallIncreasePriorityContentCondition()
        {
             string? input = "Failure";
            DateTime datecreated = DateTime.UtcNow - TimeSpan.FromMinutes(59);

            Assert.That(() => _calculatePriority.Calculate(Priority.Medium, datecreated, input) == Priority.High);
        }

        [Test]
        public void ShallIncreasePriorityTimeCondition()
        {
            string? input = "test";
            DateTime datecreated = DateTime.UtcNow - TimeSpan.FromMinutes(60);

            Assert.That(() => _calculatePriority.Calculate(Priority.Medium, datecreated, input) == Priority.High);
        }

        [Test]
        public void ShallNotIncreasePriorityNoCondition()
        {
            string? input = "test";
            DateTime datecreated = DateTime.UtcNow - TimeSpan.FromMinutes(59);

            Assert.That(() => _calculatePriority.Calculate(Priority.Medium, datecreated, input) == Priority.Medium);
        }

    }
}
