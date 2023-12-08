using EZTicket.Models;

namespace EZTicket.Repository;

// Interface for TicketRepository
public interface ITicketRepository
{
    public Task<List<Ticket>?> GetTicketsAsync();
    public Task<Ticket?> GetTicketAsync(int id);
    public Task<Ticket> AddTicketAsync(Ticket ticket);
    public Task<Ticket?> UpdateTicketAsync(Ticket ticket);
    public Task<Ticket?> DeleteTicketAsync(int id);
    public Task<List<TicketNote>?> GetTicketNotesAsync(int id);
    public Task<TicketNote?> AddTicketNoteAsync(int id, TicketNote note);
}