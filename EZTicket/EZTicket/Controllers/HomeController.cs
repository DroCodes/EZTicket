using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EZTicket.Models;

namespace EZTicket.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TicketContext _context;

    public HomeController(ILogger<HomeController> logger, TicketContext ctx)
    {
        _logger = logger;
        _context = ctx;
    }

    public IActionResult Index()
    {
        ViewBag.PendingTickets = _context.PendingTickets.ToList();
        ViewBag.ActiveTickets = _context.ActiveTickets.ToList();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}