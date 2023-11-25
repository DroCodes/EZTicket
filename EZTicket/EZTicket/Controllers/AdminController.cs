using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class AdminController : Controller
{
    // GET
    public IActionResult Admin()
    {
        return View();
    }
}