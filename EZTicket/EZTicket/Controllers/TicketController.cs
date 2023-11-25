using EZTicket.Models;
using EZTicket.Repository;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace EZTicket.Controllers;

public class TicketController : Controller
{
    private readonly TicketContext _context;
    private readonly IPendingTicketRepository _pendingRepo;
    private readonly IActiveTicketRepository _activeRepo;
    
    public TicketController(TicketContext context, IPendingTicketRepository pendingRepo, IActiveTicketRepository activeRepo)
    {
        _pendingRepo = pendingRepo;
        _activeRepo = activeRepo;
        _context = context;
    }
    // GET
    public IActionResult Ticket()
    {
        return View();
    }

    public IActionResult CreateTicket()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket()
    {
        var ticket = new PendingTickets
        {
            Name = Request.Form["name"],
            Description = Request.Form["description"],
            ServiceType = Request.Form["service-type"],
            Priority = Convert.ToInt32(Request.Form["priority"]),
            CreatedBy = Request.Form["created-by"],
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now
        };
        
        await _pendingRepo.AddPendingTicketAsync(ticket);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> PendingTicket(int id)
    {
        try
        {
            var ticket = await _pendingRepo.GetPendingTicketAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            ViewBag.ticket = ticket;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View();
    }

    public async Task<IActionResult> AssignTicket(int id)
    {
        try
        {
            var ticket = await _pendingRepo.GetPendingTicketAsync(id);

            if (ticket != null)
            {
                var assignedTicket = new ActiveTickets
                {
                    Name = ticket.Name,
                    Description = ticket.Description,
                    ServiceType = ticket.ServiceType,
                    Priority = ticket.Priority,
                    CreatedBy = ticket.CreatedBy,
                    AssignedTo = "test",
                    DateCreated = ticket.DateCreated,
                    DateUpdated = ticket.DateUpdated
                };

                var result = await _activeRepo.AddActiveTicketAsync(assignedTicket);

                await _pendingRepo.DeletePendingTicketAsync(id);

                // transaction.Commit();
            }
            else
            {
                // transaction.Rollback(); // Rollback if the ticket is not found
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            // transaction.Rollback(); // Rollback in case of any exception
            // Log the exception
            Console.WriteLine($"Error assigning ticket: {ex}");
            // You might want to throw the exception again if you want to propagate it
        }

        return RedirectToAction("Index", "Home");
    }

}