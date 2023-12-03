using EZTicket.Models;
using EZTicket.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Controllers;

public class AdminController : Controller
{
    private readonly IActiveTicketRepository _activeTicketRepository;
    private readonly IPendingTicketRepository _pendingTicketRepository;
    private readonly UserManager<UserModel> _userManager;
    
    public AdminController(IActiveTicketRepository activeRepo, IPendingTicketRepository pendingRepo, UserManager<UserModel> userManager)
    {
        _userManager = userManager;
        _activeTicketRepository = activeRepo;
        _pendingTicketRepository = pendingRepo;
    }
    // GET
    public async Task<IActionResult> Admin()
    {
        var getUser = await _userManager.GetUserAsync(User);
        var verifyUser = await _userManager.IsInRoleAsync(getUser, "Admin");
        if (!verifyUser)
        {
            return RedirectToAction("Index", "Home");
        }
        
        ViewBag.ActiveTickets = await _activeTicketRepository.GetActiveTicketsAsync();
        ViewBag.PendingTickets = await _pendingTicketRepository.GetPendingTicketsAsync();
        
        return View();
    }
    
    public async Task<IActionResult> UserManagement()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }
    
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.DeleteAsync(user);
        return RedirectToAction("UserManagement");
    }
    
    public async Task<IActionResult> MakeAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.AddToRoleAsync(user, "Admin");
        return RedirectToAction("UserManagement");
    }
    
    public async Task<IActionResult> RemoveAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        await _userManager.RemoveFromRoleAsync(user, "Admin");
        return RedirectToAction("UserManagement");
    }
    
    public async Task<IActionResult> DeleteActiveTicket(int id)
    {
        try
        {
            await _activeTicketRepository.DeleteActiveTicketAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("Admin");
    }
    
    public async Task<IActionResult> DeletePendingTicket(int id)
    {
        try
        {
            await _pendingTicketRepository.DeletePendingTicketAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return RedirectToAction("Admin");
    }
}