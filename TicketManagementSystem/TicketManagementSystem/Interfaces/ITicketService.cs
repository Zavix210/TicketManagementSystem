using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem
{
    public interface ITicketService
    {
        int CreateTicket(string t, Priority p, string assignedTo, string desc, DateTime d, bool isPayingCustomer);

        void AssignTicket(int id, string username);
    }
}
