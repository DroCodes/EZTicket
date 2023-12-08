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
    public DbSet<Ticket> Ticket { get; set; }
    public DbSet<TicketNote> TicketNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // defines the relationship between Ticket and TicketNotes with active tickets as the parent and ticket notes as the child with a foreign key of TicketId
        modelBuilder.Entity<Ticket>()
            .HasMany(n => n.Notes)
            .WithOne(n => n.Ticket)
            .HasForeignKey(n => n.TicketId)
            .HasPrincipalKey(n => n.Id);
    }
    
    // creates the admin user if it does not exist
    public static async Task CreateAdminUser(IServiceProvider serviceProvider)
    {
        using (var scoped = serviceProvider.CreateScope())
        {
            UserManager<UserModel> userManager = scoped.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            RoleManager<IdentityRole> roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string pwd = "Password123!";
            string roleName = "Admin";
            string emailAddress = "admin@email.com";
            string phoneNumber = "1234567890";
            string firstName = "Admin";
            string lastName = "Admin";

            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (await userManager.FindByNameAsync(username) == null)
            {
                UserModel user = new UserModel() { UserName = username, EmailAddress = emailAddress, PhoneNumber = phoneNumber, FirstName = firstName, LastName = lastName };
                var result = await userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}