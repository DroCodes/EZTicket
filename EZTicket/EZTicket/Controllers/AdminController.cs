using EZTicket.Models;
using EZTicket.Repository;
using EZTicket.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ITicketRepository _repo;
    private readonly UserManager<UserModel> _userManager;

    public AdminController(ITicketRepository repo, UserManager<UserModel> userManager)
    {
        _userManager = userManager;
        _repo = repo;
    }

    // GET
    public async Task<IActionResult> Admin()
    {
        try
        {
            // gets all pending and active tickets from the database
            // var pending = await _pendingTicketRepository.GetPendingTicketsAsync();
            var tickets = await _repo.GetTicketsAsync();

            // adds the tickets to the services which is a priority queue to sort the tickets
            PriorityTicketService pendingTicketQueue = new();
            PriorityTicketService activeTicketQueue = new();

            if (tickets != null)
            {
                foreach (var ticket in tickets)
                {
                    if (!ticket.IsClosed)
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
            }

            ViewBag.PendingTickets = pendingTicketQueue.GetTickets();
            ViewBag.ActiveTickets = activeTicketQueue.GetTickets();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return View();
    }

    public async Task<IActionResult> UserManagement()
    {
        // gets all users from the database
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> DeleteUser(string id)
    {
        // deletes the user from the database
        try
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("UserManagement");
            }

            await _userManager.DeleteAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return RedirectToAction("UserManagement");
    }

    public async Task<IActionResult> DeleteTicket(int id)
    {
        // deletes the active ticket from the database
        try
        {
            await _repo.DeleteTicketAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return RedirectToAction("Admin");
    }
}