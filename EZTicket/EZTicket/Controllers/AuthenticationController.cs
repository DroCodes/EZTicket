using EZTicket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EZTicket.Controllers;

public class AuthenticationController : Controller
{
    private UserManager<UserModel> _userManager;
    private SignInManager<UserModel> _signInManager;
    
    public AuthenticationController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
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
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Invalid Model State");
            
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {error.ErrorMessage}");
            }
            
            return View(loginModel);
        }
        
        var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, loginModel.RememberMe, false);
        
        if (result.Succeeded)
        {
            Console.WriteLine("Succeeded");
            if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
            {
                Console.WriteLine("Redirecting to ReturnUrl");
                // return Redirect(loginModel.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine("Redirecting to Index");
            return RedirectToAction("Index", "Home");
        }
        
        if (!result.Succeeded)
        {
            Console.WriteLine($"SignIn failed: {result}");
        }

        ModelState.AddModelError("", "Invalid UserName or Password");
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
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Invalid Model State");

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {error.ErrorMessage}");
            }

            return View(registerModel);
        }

        var user = new UserModel
        {
            UserName = registerModel.UserName,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            EmailAddress = registerModel.EmailAddress,
            PhoneNumber = registerModel.PhoneNumber
        };
        
        var result = await _userManager.CreateAsync(user, registerModel.Password);
        
        if (result.Succeeded)
        {
            Console.WriteLine("Succeeded");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        return View(registerModel);
    }
}