using EZTicket.Exceptions;
using EZTicket.Models;

namespace EZTicket.Services;

public class ActiveTicketService
{
    private ActiveTicketNode? _head = null;
    
    public ActiveTicketService()
    {
        
    }

    public bool IsEmpty()
    {
        return _head == null;
    }

    public void AddTicket(ActiveTickets ticket)
    {
        ActiveTicketNode newNode = new ActiveTicketNode
        {
            Ticket = ticket,
            Next = _head
        };
    
        _head = newNode;
    }
    
    
    public ActiveTickets RemoveTicket()
    {
        if (IsEmpty())
        {
            throw new ListEmptyException("Active Ticket List is empty.");
        }

        ActiveTickets removedTicket = _head.Ticket;
        _head = _head.Next;

        return removedTicket;
    }

    public ActiveTickets Peek()
    {
        if (IsEmpty())
        {
            throw new ListEmptyException("Active Ticket List is empty.");
        }

        return _head.Ticket;
    }

    public List<ActiveTickets> GetTickets()
    {
        List<ActiveTickets> tickets = new List<ActiveTickets>();
        ActiveTicketNode current = _head;

        while (current != null)
        {
            tickets.Add(current.Ticket);
            current = current.Next;
        }

        return tickets;
    }
}