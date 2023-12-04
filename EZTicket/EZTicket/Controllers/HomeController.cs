using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EZTicket.Models;
using EZTicket.Repository;
using EZTicket.Services;

namespace EZTicket.Controllers;

public class HomeController : Controller
{
    private readonly IActiveTicketRepository _activeTicketRepository;
    private readonly IPendingTicketRepository _pendingTicketRepository;

    public HomeController(TicketContext ctx, IActiveTicketRepository activeTicketRepository, IPendingTicketRepository pendingTicketRepository)
    {
        _pendingTicketRepository = pendingTicketRepository;
        _activeTicketRepository = activeTicketRepository;
    }

    public async Task<IActionResult> Index()
    {
        var pending = await _pendingTicketRepository.GetPendingTicketsAsync();
        var activeTickets = await _activeTicketRepository.GetActiveTicketsAsync();
        
        PendingTicketService pendingTicketService = new();

        if (pending != null)
        {
            foreach (var ticket in pending)
            {
                pendingTicketService.AddTicket(ticket);
            }
        }
        
        ActiveTicketService activeTicketService = new();
        
        if (activeTickets != null)
        {
            foreach (var ticket in activeTickets)
            {
                if (!ticket.IsClosed)
                {
                    activeTicketService.AddTicket(ticket);
                }
            }
        }
        
        ViewBag.PendingTickets = pendingTicketService.GetTickets();
        ViewBag.ActiveTickets = activeTicketService.GetTickets();
        
        // ViewBag.PendingTickets = _context.PendingTickets.ToList();
        // ViewBag.ActiveTickets = _context.ActiveTickets.ToList();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}