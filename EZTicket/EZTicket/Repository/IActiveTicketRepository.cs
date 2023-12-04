using EZTicket.Models;

namespace EZTicket.Repository;

public interface IActiveTicketRepository
{
    public Task<List<ActiveTickets>?> GetActiveTicketsAsync();
    public Task<ActiveTickets?> GetActiveTicketAsync(int id);
    public Task<ActiveTickets> AddActiveTicketAsync(ActiveTickets ticket);
    public Task<ActiveTickets?> UpdateActiveTicketAsync(ActiveTickets ticket);
    public Task<ActiveTickets?> DeleteActiveTicketAsync(int id);
    public Task<List<TicketNote>?> GetTicketNotesAsync(int id);
    public Task<TicketNote?> AddTicketNoteAsync(int id, TicketNote note);
}