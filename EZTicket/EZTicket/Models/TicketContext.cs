using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EZTicket.Models;

public class TicketContext : IdentityDbContext<UserModel>
{
    public TicketContext(DbContextOptions<TicketContext> options) : base(options)
    {
    }
    
    public DbSet<UserModel> Users { get; set; }
    public DbSet<PendingTickets> PendingTickets { get; set; }
    public DbSet<ActiveTickets> ActiveTickets { get; set; }
    public DbSet<TicketHistory> TicketHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public static async Task CreateAdminUser(IServiceProvider serviceProvider)
    {
        using (var scoped = serviceProvider.CreateScope())
        {
            Console.WriteLine("Creating admin user");
            UserManager<UserModel> userManager = scoped.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            RoleManager<IdentityRole> roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string pwd = "Password123!";
            string roleName = "Admin";

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                Console.WriteLine($"Created role {roleName}");
            }

            if (await userManager.FindByNameAsync(username) == null)
            {
                UserModel user = new UserModel() { UserName = username };
                var result = await userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    Console.WriteLine("success");
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}