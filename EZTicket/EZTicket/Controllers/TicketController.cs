using EZTicket.Models;
using EZTicket.Repository;
using EZTicket.Services;
using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class TicketController : Controller
{
    private readonly IPendingTicketRepository _pendingRepo;
    private readonly IActiveTicketRepository _activeRepo;
    
    public TicketController(IPendingTicketRepository pendingRepo, IActiveTicketRepository activeRepo)
    {
        _pendingRepo = pendingRepo;
        _activeRepo = activeRepo;
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
                    IsClosed = false,
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
            
            var notes = await _activeRepo.GetTicketNotesAsync(id);

            if (notes != null)
            {
                ViewBag.Notes = notes;
            }
            
            ViewBag.Ticket = ticket;
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
                ticket.IsClosed = true;
                ticket.CompletedBy = User.Identity.Name;
                ticket.DateCompleted = DateTime.Now;

                await _activeRepo.UpdateActiveTicketAsync(ticket);
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
            var tickets = await _activeRepo.GetActiveTicketsAsync();
            
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
            var ticket = await _activeRepo.GetActiveTicketAsync(id);
            
            var notes = await _activeRepo.GetTicketNotesAsync(id);
            
            ViewBag.Notes = notes;
            return View("ClosedTicket", ticket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddNote()
    {
        try
        {
            var id = Convert.ToInt32(Request.Form["id"]);
            var ticket = await _activeRepo.GetActiveTicketAsync(id);
            
            if (ticket == null)
            {
                Console.WriteLine("not found");
                return NotFound();
            }
            
            var note = new TicketNote
            {
                UserName = User.Identity.Name,
                Note = Request.Form["Note"],
                Created = DateTime.Now,
                TicketId = id
            };
            
            ticket.DateUpdated = DateTime.Now;

            await _activeRepo.AddTicketNoteAsync(id, note);
            await _activeRepo.UpdateActiveTicketAsync(ticket);
            
            return RedirectToAction("ActiveTicket", "Ticket", new {id = id});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> UserDashboard()
    {
        try
        {
            var user = User.Identity.Name;
            var tickets = await _activeRepo.GetActiveTicketsAsync();
            
            if (tickets == null)
            {
                return NotFound();
            }

            var activeTicket = new ActiveTicketService();

            foreach (var ticket in tickets)
            {
                if (ticket.AssignedTo == user)
                {
                    activeTicket.AddTicket(ticket);
                }
            }
            
            ViewBag.activeTickets = activeTicket.GetTickets();
            
            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            RedirectToAction("Index", "Home");
        }

        return View();
    }
}