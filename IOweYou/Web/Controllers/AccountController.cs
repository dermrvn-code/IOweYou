using System.Security.Claims;
using IOweYou.Models;
using IOweYou.ViewModels.Account;
using IOweYou.Web.Services;
using IOweYou.Web.Services.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IOweYou.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;
    private readonly PasswordHasher<object> _passwordHasher;

    public AccountController(ILogger<AccountController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        _passwordHasher = new PasswordHasher<object>();
    }
    
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel register)
    {
        if (ModelState.IsValid)
        {
            
            var existingUserEmail = await _userService.FindByEmail(register.Email);
            if (existingUserEmail != null){
                ViewBag.ErrorMessage = "Email is already taken";
                return View();
            }
            
            var existingUserUsername = await _userService.FindByUsername(register.Username);
            if (existingUserUsername != null)
            {
                ViewBag.ErrorMessage = "Username is already taken";
                return View();
            }

            var passwordHashed = _passwordHasher.HashPassword(null, register.Password);
            var user = new User(register.Username, register.Email, passwordHashed);
            
            await _userService.Add(user);
            
            return Redirect("/login");
            
        }
        
        return View();
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginViewModel login)
    {
        if (!ModelState.IsValid)
            return View();

        var user = await _userService.FindByUsername(login.Username);
        if (user is null)
        {
            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }
        
        var passwordResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, login.Password);
        if (passwordResult != PasswordVerificationResult.Success)
        {
            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }
        
        var claims = user.ToClaims();
        var claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        return Redirect("/");
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }

    [HttpGet("forgotpassword")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel fp)
    {
        if (!ModelState.IsValid)
            return View();
        
        var user = _userService.FindByEmail(fp.Email);
        //if (user is not null)
            
        return View();
    }
}