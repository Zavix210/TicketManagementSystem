using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Interfaces
{
    public interface ICalculatePriority
    {
        Priority Calculate(Priority p, DateTime creation, string T);
    }
}
