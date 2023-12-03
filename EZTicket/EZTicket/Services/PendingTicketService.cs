using EZTicket.Exceptions;
using EZTicket.Models;

namespace EZTicket.Services;

public class PendingTicketService
{
    private List<PendingTickets> _pendingTicketQueue = new();

    public PendingTicketService()
    {

    }
    
    public List<PendingTickets> GetTickets()
    {
        return _pendingTicketQueue;
    }

    public PendingTickets Peek()
    {
        return _pendingTicketQueue.First();
    }
    
    public bool isEmpty()
    {
        return _pendingTicketQueue.Count == 0;
    }
    
    public void AddTicket(PendingTickets? ticket)
    {
        if (ticket == null)
        {
            return;
        }

        if (_pendingTicketQueue.Count == 0 || ticket.Priority == 3)
        {
            _pendingTicketQueue.Insert(0, ticket);
        }
        else if (ticket.Priority == 2)
        {
            _pendingTicketQueue.Insert(1, ticket);
        }
        else
        {
            _pendingTicketQueue.Add(ticket);
        }
    }
    
    public PendingTickets RemoveTicket()
    {
        if (isEmpty())
        {
            throw new ListEmptyException();
        }
        PendingTickets firstItem = _pendingTicketQueue.First();
        
        _pendingTicketQueue.Remove(firstItem);
        return firstItem;
    }
}