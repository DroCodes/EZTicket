using Microsoft.AspNetCore.Mvc;
using EZTicket.Models;
using EZTicket.Repository;
using EZTicket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace EZTicket.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ITicketRepository _ticketRepository;

    public HomeController(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }
    
    public async Task<IActionResult> Index()
    {
        // Get all active tickets
        var tickets = await _ticketRepository.GetTicketsAsync();
        
        // Add pending tickets to pending ticket service which is a priority queue
        PriorityTicketService pendingTicketQueue = new();
        PriorityTicketService activeTicketQueue = new();

        if (tickets != null)
        {
            foreach (var ticket in tickets)
            {
                if (ticket.IsPending)
                {
                    pendingTicketQueue.AddTicket(ticket);
                }
                else
                {
                    activeTicketQueue.AddTicket(ticket);
                }
            }
        }
        
        ViewBag.PendingTickets = pendingTicketQueue.GetTickets();
        ViewBag.ActiveTickets = activeTicketQueue.GetTickets();
        
        return View();
    }
}