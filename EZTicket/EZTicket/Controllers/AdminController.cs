using EZTicket.Models;
using EZTicket.Repository;
using EZTicket.Services;
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