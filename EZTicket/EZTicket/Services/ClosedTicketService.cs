using EZTicket.Exceptions;
using EZTicket.Models;

namespace EZTicket.Services;

public class ClosedTicketService
{
    private ClosedTicketNode? _head = null;
    
    public ClosedTicketService()
    {
        
    }

    // Checks if the list is empty
    public bool IsEmpty()
    {
        return _head == null;
    }

    // Adds a ticket to the list
    public void AddTicket(Ticket ticket)
    {
        ClosedTicketNode newNode = new ClosedTicketNode
        {
            Ticket = ticket,
            Next = _head
        };
    
        _head = newNode;
    }
    
    // Removes a ticket from the list
    public Ticket RemoveTicket()
    {
        if (IsEmpty())
        {
            throw new ListEmptyException("Active Ticket List is empty.");
        }

        Ticket removedTicket = _head.Ticket;
        _head = _head.Next;

        return removedTicket;
    }

    // Gets the ticket at the top of the list
    public Ticket Peek()
    {
        if (IsEmpty())
        {
            throw new ListEmptyException("Active Ticket List is empty.");
        }

        return _head.Ticket;
    }

    // Gets all tickets in the list
    public List<Ticket> GetTickets()
    {
        List<Ticket> tickets = new List<Ticket>();
        ClosedTicketNode current = _head;

        while (current != null)
        {
            tickets.Add(current.Ticket);
            current = current.Next;
        }

        return tickets;
    }
}