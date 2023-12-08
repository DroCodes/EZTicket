using EZTicket.Models;

namespace EZTicket.Services;

// Node for ClosedTicketService
public class ClosedTicketNode
{
        public Ticket Ticket { get; set; }
        public ClosedTicketNode Next { get; set; }
}