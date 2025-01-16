using System.Security.Claims;
using IOweYou.Models;
using IOweYou.ViewModels.Account;
using IOweYou.Web.Services.Mail;
using IOweYou.Web.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IOweYou.Web.Controllers;
using Microsoft.AspNetCore.Mvc;


public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;
    private readonly PasswordHasher<object> _passwordHasher;
    private readonly IMailService _mailService;

    public AccountController(ILogger<AccountController> logger, IUserService userService, IMailService mailService)
    {
        _logger = logger;
        _userService = userService;
        _passwordHasher = new PasswordHasher<object>();
        _mailService = mailService;
    }
    
    [AllowAnonymous]
    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterViewModel register)
    {
        if (!ModelState.IsValid) 
            return View();
        
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
    
    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [AllowAnonymous]
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
        
        _mailService.SendPasswortResetMail(user);
        
        
        return Redirect("/");
    }
    
    [AllowAnonymous]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }
    
    [AllowAnonymous]
    [Route("forgotpassword")]
    public IActionResult ForgotPassword()
    {
        return View();
    }
    
    [AllowAnonymous]
    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel fp)
    {
        if (!ModelState.IsValid)
            return View();
        
        var user = _userService.FindByEmail(fp.Email);
            
        return View();
    }
    
    [AllowAnonymous]
    [Route("changepassword/{token?}")]
    public async Task<IActionResult> ChangePassword(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            ViewBag.Token = token;
            return View();
        }
        else if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            return View();
        }
        return NotFound();
        
    }
    
    [AllowAnonymous]
    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword([FromForm]ChangePasswordViewModel changePassword)
    {
        if (!ModelState.IsValid)
            return View();
        
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("logout");

        if (!changePassword.UseToken)
        {
            var passwordResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, changePassword.OldPassword);
            if (passwordResult != PasswordVerificationResult.Success)
            {
                ViewBag.ErrorMessage = "Wrong password";
                return View();
            }

            if (changePassword.NewPassword != changePassword.ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match";
                return View();
            }
        }
        else
        {
            
        }
        
        var passwordEqual = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, changePassword.NewPassword);
        if (passwordEqual == PasswordVerificationResult.Success)
        {
            ViewBag.ErrorMessage = "New password cannot be the same as old password";
            return View();
        }
        
        var passwordHashed = _passwordHasher.HashPassword(null, changePassword.NewPassword);
        user.PasswordHash = passwordHashed;

        if (!await _userService.Update(user))
        {
            ViewBag.ErrorMessage = "Password could not be changed";
            return View();
        }
        
        TempData["InfoBanner"] = "Successfully changed password";
        return Redirect("/account");
    }

    [Route("changeusername")]
    public IActionResult ChangeUsername()
    {
        return View();
    }

    [HttpPost("changeusername")]
    public async Task<IActionResult> ChangeUsername([FromForm] ChangeUsernameViewModel changeUsername)
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("logout");

        if (user.Username == changeUsername.Username)
        {
            ViewBag.ErrorMessage = "Username cannot be the same";
            return View();
        }
        
        var possibleUser = await _userService.FindByUsername(changeUsername.Username);
        if (possibleUser != null)
        {
            ViewBag.ErrorMessage = "Username is already taken";
            return View();
        }
        
        user.Username = changeUsername.Username;

        if (!await _userService.Update(user))
        {
            ViewBag.ErrorMessage = "Username could not be changed";
            return View();
        }
        
        TempData["InfoBanner"] = "Successfully changed username to " + changeUsername.Username;
        return Redirect("/account");
        
    }
    

    [Route("changeemail")]
    public IActionResult ChangeEmail()
    {
        return View();
    }

    [HttpPost("changeemail")]
    public async Task<IActionResult> ChangeEmail([FromForm] ChangeEmailViewModel changeEmail)
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("logout");

        if (user.Email == changeEmail.Email)
        {
            ViewBag.ErrorMessage = "Email cannot be the same";
            return View();
        }
        
        var possibleUser = await _userService.FindByEmail(changeEmail.Email);
        if (possibleUser != null)
        {
            ViewBag.ErrorMessage = "Email is already in use";
            return View();
        }
        
        user.Email = changeEmail.Email;

        if (!await _userService.Update(user))
        {
            ViewBag.ErrorMessage = "Email could not be changed";
            return View();
        }
        
        TempData["InfoBanner"] = "Successfully changed email to " + changeEmail.Email;
        return Redirect("/account");
    }
    

    [Route("deleteaccount")]
    public async Task<IActionResult> DeleteAccount()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("logout");

        if (!await _userService.Delete(user.ID))
        {
            TempData["InfoBanner"] = "Account could not be deleted";
            return Redirect("/account");
        }
        
        TempData["InfoBanner"] = "Your account has been deleted";
        return Redirect("/login");
    }
    
    [Route("account")]
    public async Task<IActionResult> Account()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("logout");
        
        return View(user);
    }
}