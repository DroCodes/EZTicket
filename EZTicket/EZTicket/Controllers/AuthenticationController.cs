using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class AuthenticationController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(int id)
    {
        return RedirectToAction("Index", "Home");
    }
}