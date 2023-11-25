using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class TicketController : Controller
{
    // GET
    public IActionResult Ticket()
    {
        return View();
    }
}