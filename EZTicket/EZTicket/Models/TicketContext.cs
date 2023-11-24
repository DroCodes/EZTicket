using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Models;

public class TicketContext : IdentityDbContext<UserModel>
{
    public TicketContext(DbContextOptions<TicketContext> options) : base(options)
    {
    }
    
    public DbSet<PendingTickets> PendingTickets { get; set; }
    public DbSet<ActiveTickets> ActiveTickets { get; set; }
    public DbSet<TicketHistory> TicketHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}