using System;
using System.Collections.Generic;
using TicketManagementSystem.Interfaces;
using System.Linq;
namespace TicketManagementSystem
{
    public class CalculatePriority : ICalculatePriority
    {
        private readonly List<string> keywords = new List<string>() {
            "Crash",
            "Important",
            "Failure"
            };

        public Priority Calculate(Priority p, DateTime creation, string T)
        {
            if (UpgradeBasedOnTime(creation) || UpgradeBasedonContent(T))
                p = IncreasePriority(p);
            return p;
        }

        public bool UpgradeBasedOnTime(DateTime creation)
        {
            // this should come from a interface to fake out the UTC.NOW to properlly allow testing
            return creation < DateTime.UtcNow - TimeSpan.FromHours(1);
        }

        public bool UpgradeBasedonContent(string t)
        {
            return keywords.Any(x => x.Contains(t));
        }

        public Priority IncreasePriority(Priority p)
        {
            switch (p)
            {
                case Priority.Low:
                    p = Priority.Medium;
                    break;
                case Priority.Medium:
                    p = Priority.High;
                    break;
            }
            return p;
        }

    }
}
