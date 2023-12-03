using EZTicket.Models;

namespace EZTicket.Services;

public class ActiveTicketNode
{
        public ActiveTickets Ticket { get; set; }
        public ActiveTicketNode Next { get; set; }
}