using EZTicket.Models;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Repository;

public class ActiveTicketRepository : IActiveTicketRepository
{
    private readonly TicketContext _context;
    
    public ActiveTicketRepository(TicketContext context)
    {
        _context = context;
    }
    
    public async Task<List<ActiveTickets>?> GetActiveTicketsAsync()
    {
        try
        {
            var tickets = await _context.ActiveTickets.ToListAsync();
            return tickets.Count > 0 ? tickets : null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ActiveTickets?> GetActiveTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.ActiveTickets.FindAsync(id); 
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ActiveTickets> AddActiveTicketAsync(ActiveTickets ticket)
    {
        try
        {
            await _context.ActiveTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ActiveTickets?> UpdateActiveTicketAsync(ActiveTickets ticket)
    {
        try
        {
            _context.Update(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ActiveTickets?> DeleteActiveTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.ActiveTickets.FindAsync(id);

            if (ticket == null)
            {
                return null;
            }
            _context.ActiveTickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<TicketNote>?> GetTicketNotesAsync(int id)
    {
        try
        {
            var notes = await _context.TicketNotes.Where(n => n.TicketId == id).ToListAsync();
            
            return notes.Count > 0 ? notes : null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<TicketNote?> AddTicketNoteAsync(int id, TicketNote note)
    {
        try
        {
            var ticket = await _context.ActiveTickets.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }

            var newNote = await _context.TicketNotes.AddAsync(note);
            
            ticket.Notes ??= new List<TicketNote>();
            
            ticket.Notes.Add(note);
            await _context.SaveChangesAsync();
            
            return note;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}