using EZTicket.Models;
using EZTicket.Repository;
using EZTicket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

[Authorize]
public class TicketController : Controller
{
    private readonly ITicketRepository _repo;
    
    public TicketController(ITicketRepository repo)
    {
        _repo = repo;
    }
    
    public IActionResult CreateTicket()
    {
        // returns the CreateTicket view
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket()
    {
        // new Ticket Model
        try
        {
            var ticket = new Ticket
            {
                Name = Request.Form["Name"],
                Description = Request.Form["Description"],
                ServiceType = Request.Form["ServiceType"],
                Priority = Convert.ToInt32(Request.Form["Priority"]),
                IsPending = true,
                CreatedBy = User.Identity.Name,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            // adds the ticket to the database
            await _repo.AddTicketAsync(ticket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> PendingTicket(int id)
    {
        try
        {
            // gets the ticket from the database
            var ticket = await _repo.GetTicketAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }

            if (ticket.IsPending)
            {
                // returns the PendingTicket view with the ticket
                ViewBag.ticket = ticket;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View();
    }

    public async Task<IActionResult> AssignTicket(int id)
    {
        // Assigns the ticket to the current user
        try
        {
            // gets the ticket from the database
            var ticket = await _repo.GetTicketAsync(id);

            if (ticket != null)
            {
                // creates a new ActiveTicket model
                ticket.AssignedTo = User.Identity.Name;
                ticket.IsPending = false;
                ticket.DateUpdated = DateTime.Now;

                // adds the ticket to the database
                await _repo.UpdateTicketAsync(ticket);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error assigning ticket: {e}");
        }

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> ActiveTicket(int id)
    {
        try
        {
            // gets the ticket from the database
            var ticket = await _repo.GetTicketAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            // gets the notes from the database
            var notes = await _repo.GetTicketNotesAsync(id);

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
            // gets the ticket from the database
            var ticket = await _repo.GetTicketAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            // returns the UpdateTicket view with the ticket
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
    public async Task<IActionResult> UpdateTicket(Ticket ticket)
    {
        try
        {
            // updates the ticket in the database
            ticket.Name = Request.Form["Name"];
            ticket.Description = Request.Form["Description"];
            ticket.ServiceType = Request.Form["ServiceType"];
            ticket.AssignedTo = Request.Form["AssignedTo"];
            ticket.CreatedBy = Request.Form["CreatedBy"];
            ticket.Priority = Convert.ToInt32(Request.Form["Priority"]);
            ticket.DateCreated = Convert.ToDateTime(Request.Form["DateCreated"]);
            ticket.DateUpdated = DateTime.Now;

            // updates the ticket in the database
            await _repo.UpdateTicketAsync(ticket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> CloseTicket(int id)
    {
        try
        {
            // gets the ticket from the database
            var ticket = await _repo.GetTicketAsync(id);

            if (ticket != null)
            {
                // closes the ticket
                ticket.IsClosed = true;
                ticket.CompletedBy = User.Identity.Name;
                ticket.DateCompleted = DateTime.Now;

                await _repo.UpdateTicketAsync(ticket);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error closing ticket: {e}");
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> TicketHistory()
    {
        try
        {
            // gets the tickets from the database
            var tickets = await _repo.GetTicketsAsync();
            
            if (tickets == null)
            {
                return NotFound();
            }
            
            var closedTickets = new ClosedTicketService();
            
            // adds the tickets to the ClosedTicketService
            foreach (var ticket in tickets)
            {
                if (ticket.IsClosed)
                {
                    closedTickets.AddTicket(ticket);
                }
            }
            
            // sorts ticket history
            var sort = new InsertionSort();
            var sortedTickets = sort.SortByDate(tickets);
            
            // returns the TicketHistory view with the tickets
            ViewBag.ticketHistory = sortedTickets;
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
            // gets the ticket from the database
            var ticket = await _repo.GetTicketAsync(id);
            // gets the notes from the database
            var notes = await _repo.GetTicketNotesAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            // returns the ClosedTicket view with the ticket and notes
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
            // gets the ticket from the database
            var id = Convert.ToInt32(Request.Form["id"]);
            var ticket = await _repo.GetTicketAsync(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            // creates a new TicketNote model
            var note = new TicketNote
            {
                UserName = User.Identity.Name,
                Note = Request.Form["Note"],
                Created = DateTime.Now,
                TicketId = id
            };
            
            // updates the ticket in the database
            ticket.DateUpdated = DateTime.Now;

            // adds the note to the database
            await _repo.AddTicketNoteAsync(id, note);
            // updates the ticket in the database
            await _repo.UpdateTicketAsync(ticket);
            
            return RedirectToAction("ActiveTicket", "Ticket", new {id = id});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> UserDashboard()
    {
        try
        {
            // gets the username of the user
            var user = User.Identity.Name;
            // gets the tickets from the database
            var tickets = await _repo.GetTicketsAsync();
            
            if (tickets == null)
            {
                return NotFound();
            }

            // creates a new ClosedTicketService
            var activeTicket = new PriorityTicketService();

            // adds the tickets to the ClosedTicketService
            foreach (var ticket in tickets)
            {
                if (ticket.AssignedTo == user && !ticket.IsClosed && !ticket.IsPending)
                {
                    activeTicket.AddTicket(ticket);
                }
            }
            
            // returns the UserDashboard view with the tickets
            ViewBag.activeTickets = activeTicket.GetTickets();
            
            return View();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction("Index", "Home");
        }
    }
}