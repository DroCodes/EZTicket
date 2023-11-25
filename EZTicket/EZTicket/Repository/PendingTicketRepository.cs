using EZTicket.Models;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Repository;

public class PendingTicketRepository : IPendingTicketRepository
{
    private readonly TicketContext _context;
    
    public PendingTicketRepository(TicketContext context)
    {
        _context = context;
    }
    
    public async Task<List<PendingTickets>> GetPendingTicketsAsync()
    {
        try
        {
            var tickets = await _context.PendingTickets.ToListAsync();
            return tickets;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PendingTickets?> GetPendingTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.PendingTickets.FindAsync(id); 
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PendingTickets> AddPendingTicketAsync(PendingTickets ticket)
    {
        try
        {
            await _context.PendingTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PendingTickets> UpdatePendingTicketAsync(PendingTickets ticket)
    {
        try
        {
            _context.PendingTickets.Update(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PendingTickets?> DeletePendingTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.PendingTickets.FindAsync(id);

            if (ticket == null)
            {
                return null;
            }
            _context.PendingTickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}