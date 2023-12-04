// using EZTicket.Models;
// using Microsoft.EntityFrameworkCore;
//
// namespace EZTicket.Repository;
//
// public class TicketHistoryRepository : ITicketHistoryRepository
// {
//     private readonly TicketContext _context;
//     
//     public TicketHistoryRepository(TicketContext context)
//     {
//         _context = context;
//     }
//     
//     public async Task<List<TicketHistory>?> GetTicketHistory()
//     {
//
//         try
//         {
//             return await _context.TicketHistory.ToListAsync() ?? null;
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
//
//     public async Task<TicketHistory?> GetTicketHistoryAsync(int id)
//     {
//         try
//         {
//             var ticket = await _context.TicketHistory.FindAsync(id);
//
//             return ticket ?? null;
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
//
//     public async Task<TicketHistory> AddTicketHistoryAsync(TicketHistory ticketHistory)
//     {
//         try
//         {
//             await _context.TicketHistory.AddAsync(ticketHistory);
//             await _context.SaveChangesAsync();
//             return ticketHistory;
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
//
//     public async Task<List<TicketNote>?> GetTicketNotesAsync(int id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task<TicketNote?> AddTicketNoteAsync(int id, TicketNote note)
//     {
//         try
//         {
//             var ticket = await _context.TicketHistory.FindAsync(id);
//
//             if (ticket == null)
//             {
//                 return null;
//             }
//
//             ticket.Notes.Add(note);
//             await _context.SaveChangesAsync();
//             return note;
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
// }