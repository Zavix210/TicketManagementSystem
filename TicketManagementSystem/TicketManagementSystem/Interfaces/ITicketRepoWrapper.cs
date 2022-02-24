using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Interfaces
{
    public interface ITicketRepoWrapper
    {
        int CreateTicket(Ticket ticket);

        void UpdateTicket(Ticket ticket);

        Ticket GetTicket(int id);
    }
}
