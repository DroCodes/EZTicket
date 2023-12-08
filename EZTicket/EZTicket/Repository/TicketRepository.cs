using EZTicket.Models;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Repository;

public class TicketRepository : ITicketRepository
{
    private readonly TicketContext _context;
    
    public TicketRepository(TicketContext context)
    {
        _context = context;
    }
    
    // returns all active tickets as a List
    public async Task<List<Ticket>?> GetTicketsAsync()
    {
        try
        {
            var tickets = await _context.Ticket.ToListAsync();
            return tickets.Count > 0 ? tickets : null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // returns a single active ticket by id
    public async Task<Ticket?> GetTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.Ticket.FindAsync(id); 
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // adds a new active ticket
    public async Task<Ticket> AddTicketAsync(Ticket ticket)
    {
        try
        {
            await _context.Ticket.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Updates an active ticket
    public async Task<Ticket?> UpdateTicketAsync(Ticket ticket)
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

    // Deletes an active ticket
    public async Task<Ticket?> DeleteTicketAsync(int id)
    {
        try
        {
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return null;
            }
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // returns all ticket notes for a given ticket id
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

    // Adds new ticket note to a given ticket id
    public async Task<TicketNote?> AddTicketNoteAsync(int id, TicketNote note)
    {
        try
        {
            var ticket = await _context.Ticket.FindAsync(id);
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