using EZTicket.Models;

namespace EZTicket.Repository;

public interface ITicketHistoryRepository
{
    public Task<List<TicketHistory>?> GetTicketHistory();
    public Task<TicketHistory?> GetTicketHistoryAsync(int id);
    public Task<TicketHistory> AddTicketHistoryAsync(TicketHistory ticketHistory);
}