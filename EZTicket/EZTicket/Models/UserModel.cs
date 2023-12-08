using Microsoft.AspNetCore.Identity;

namespace EZTicket.Models;

public class UserModel : IdentityUser
{
    // Model for User Table
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
}