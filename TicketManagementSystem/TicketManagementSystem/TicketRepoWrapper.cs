using System;
using TicketManagementSystem.Interfaces;

namespace TicketManagementSystem
{
    public class TicketRepoWrapper : ITicketRepoWrapper
    {
        public int CreateTicket(Ticket ticket)
        {
            return TicketRepository.CreateTicket(ticket);
        }

        public void UpdateTicket(Ticket ticket)
        {
            TicketRepository.UpdateTicket(ticket);
        }

        public Ticket GetTicket(int id)
        {
            return TicketRepository.GetTicket(id);
        }
    }
}
