using EZTicket.Exceptions;
using EZTicket.Models;

namespace EZTicket.Services;

// Priority Queue for Pending Tickets
public class PriorityTicketService
{
    private readonly List<Ticket> _ticketQueue = new();

    public PriorityTicketService()
    {

    }
    
    // returns all tickets in the queue
    public List<Ticket> GetTickets()
    {
        return _ticketQueue;
    }

    // returns the ticket at the top of the queue
    public Ticket Peek()
    {
        return _ticketQueue.First();
    }
    
    // returns true if the queue is empty
    public bool isEmpty()
    {
        return _ticketQueue.Count == 0;
    }
    
    // adds a ticket to the queue based on priority level
    public void AddTicket(Ticket? ticket)
    {
        if (ticket == null)
        {
            return;
        }

        if (_ticketQueue.Count == 0 || ticket.Priority == 3)
        {
            _ticketQueue.Insert(0, ticket);
        }
        else if (ticket.Priority == 2 && _ticketQueue.Count >= 1)
        {
            _ticketQueue.Insert(1, ticket);
        }
        else
        {
            _ticketQueue.Add(ticket);
        }
    }
    
    // removes a ticket from the queue
    public Ticket RemoveTicket()
    {
        if (isEmpty())
        {
            throw new ListEmptyException();
        }
        Ticket firstItem = _ticketQueue.First();
        
        _ticketQueue.Remove(firstItem);
        return firstItem;
    }
}