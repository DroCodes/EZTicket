using EZTicket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class Account : Controller
{
    private UserManager<UserModel> _userManager;
    private SignInManager<UserModel> _signInManager;
    
    public Account(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        var model = new LoginModel { ReturnUrl = returnUrl };
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        // Checks if the model is valid
        if (!ModelState.IsValid)
        {
            // loops through the model state errors and prints them to the console
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {error.ErrorMessage}");
            }
            
            return View(loginModel);
        }
        
        try
        {
            // Attempts to sign in the user
            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password,
                loginModel.RememberMe, false);

            // Checks if the sign in succeeded
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // If the sign in failed, add an error to the model state
            ModelState.AddModelError("", "Invalid UserName or Password");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return View(loginModel);
    }
    
    [HttpGet]
    public IActionResult RegisterUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
    {
        // Checks if the model is valid
        if (!ModelState.IsValid)
        {
            // loops through the model state errors and prints them to the console
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {error.ErrorMessage}");
            }

            return View(registerModel);
        }

        // Creates a new user model
        var user = new UserModel
        {
            UserName = registerModel.UserName,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            EmailAddress = registerModel.EmailAddress,
            PhoneNumber = registerModel.PhoneNumber
        };
        
        try
        {
            // Attempts to create the user
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            // Checks if the user was created successfully
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return View(registerModel);
    }

    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        // Signs out the user
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
    
    // This is the default action for when a user is denied access to a page
    public IActionResult AccessDenied()
    {
        return View();
    }
}