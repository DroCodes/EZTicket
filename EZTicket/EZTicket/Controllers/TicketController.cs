using EZTicket.Models;
using EZTicket.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class TicketController : Controller
{
    private readonly IPendingTicketRepository _pendingRepo;
    private readonly IActiveTicketRepository _activeRepo;
    private readonly ITicketHistoryRepository _ticketHistoryRepo;
    
    public TicketController(IPendingTicketRepository pendingRepo, IActiveTicketRepository activeRepo, ITicketHistoryRepository ticketHistoryRepo)
    {
        _pendingRepo = pendingRepo;
        _activeRepo = activeRepo;
        _ticketHistoryRepo = ticketHistoryRepo;
    }
    // GET
    // public IActionResult Ticket()
    // {
    //     return View();
    // }

    public IActionResult CreateTicket()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket()
    {
        var ticket = new PendingTickets
        {
            Name = Request.Form["Name"],
            Description = Request.Form["Description"],
            ServiceType = Request.Form["ServiceType"],
            Priority = Convert.ToInt32(Request.Form["Priority"]),
            CreatedBy = User.Identity.Name,
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
                    Id = ticket.Id,
                    Name = ticket.Name,
                    Description = ticket.Description,
                    ServiceType = ticket.ServiceType,
                    Priority = ticket.Priority,
                    CreatedBy = ticket.CreatedBy,
                    AssignedTo = User.Identity.Name,
                    DateCreated = ticket.DateCreated,
                    DateUpdated = ticket.DateUpdated
                };

                var result = await _activeRepo.AddActiveTicketAsync(assignedTicket);

                await _pendingRepo.DeletePendingTicketAsync(id);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error assigning ticket: {ex}");
        }

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> ActiveTicket(int id)
    {
        try
        {
            var ticket = await _activeRepo.GetActiveTicketAsync(id);
            
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
    
    [HttpGet]
    public async Task<IActionResult> UpdateTicket(int id)
    {
        try
        {
            var ticket = await _activeRepo.GetActiveTicketAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            ViewBag.ticket = ticket;
            return View("UpdateTicket", ticket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateTicket(ActiveTickets ticket)
    {
        ticket.Name = Request.Form["Name"];
        ticket.Description = Request.Form["Description"];
        ticket.ServiceType = Request.Form["ServiceType"];
        ticket.AssignedTo = Request.Form["AssignedTo"];
        ticket.CreatedBy = Request.Form["CreatedBy"];
        ticket.Priority = Convert.ToInt32(Request.Form["Priority"]);
        ticket.DateCreated = Convert.ToDateTime(Request.Form["DateCreated"]);
        ticket.DateUpdated = DateTime.Now;
        
        await _activeRepo.UpdateActiveTicketAsync(ticket);

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> CloseTicket(int id)
    {
        try
        {
            var ticket = await _activeRepo.GetActiveTicketAsync(id);

            if (ticket != null)
            {
                var closedTicket = new TicketHistory
                {
                    Id = ticket.Id,
                    Name = ticket.Name,
                    Description = ticket.Description,
                    ServiceType = ticket.ServiceType,
                    Priority = ticket.Priority,
                    CreatedBy = ticket.CreatedBy,
                    CompletedBy = User.Identity.Name,
                    DateCreated = ticket.DateCreated,
                    DateUpdated = DateTime.Now,
                };

                var result = await _ticketHistoryRepo.AddTicketHistoryAsync(closedTicket);

                await _activeRepo.DeleteActiveTicketAsync(id);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error closing ticket: {ex}");
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> TicketHistory()
    {
        try
        {
            var tickets = await _ticketHistoryRepo.GetTicketHistory();
            
            if (tickets == null)
            {
                return NotFound();
            }
            
            ViewBag.ticketHistory = tickets;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> ClosedTicket(int id)
    {
        try
        {
            var ticket = await _ticketHistoryRepo.GetTicketHistoryAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            ViewBag.ticket = ticket;
            return View("ClosedTicket", ticket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View();
    }
}