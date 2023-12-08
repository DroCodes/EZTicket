using System.ComponentModel.DataAnnotations;

namespace EZTicket.Models;

// Model for Login Page
public class LoginModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    public string? ReturnUrl { get; set; }
    public bool RememberMe { get; set; }
}