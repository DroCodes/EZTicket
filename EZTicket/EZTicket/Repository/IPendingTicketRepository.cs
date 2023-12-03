using EZTicket.Models;

namespace EZTicket.Repository;

public interface IPendingTicketRepository
{
    public Task<List<PendingTickets>?> GetPendingTicketsAsync();
    public Task<PendingTickets?> GetPendingTicketAsync(int id);
    public Task<PendingTickets> AddPendingTicketAsync(PendingTickets ticket);
    public Task<PendingTickets> UpdatePendingTicketAsync(PendingTickets ticket);
    public Task<PendingTickets?> DeletePendingTicketAsync(int id);
}