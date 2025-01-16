using System.Security.Claims;
using IOweYou.Helper;
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
        
        bool success = await _mailService.SendRegistrationMail(user);
        if (!success)
            ViewBag.ErrorMessage = "Registration failed. Please try again later.";
        
        TempData["InfoBanner"] = "Send email to " + register.Email + ". Please verify!";
        return Redirect("/login");
        
    }

    [AllowAnonymous]
    [Route("verifyaccount/{token}")]
    public async Task<IActionResult> VerifyAccount(string token)
    {
        var t = await _userService.GetToken(token);
        if(t == null) return Redirect("/");

        if (t.TokenExpiry < DateTime.Now)
        {
            TempData["InfoBanner"] = "Token has expired";
            return Redirect("/");
        }

        var user = t.User;
        user.Verified = true;
        bool success = await _userService.Update(user);
        if (!success)
        {
            TempData["InfoBanner"] = "Could not verify account";
            return Redirect("/");
        }

        await _userService.RemoveToken(t.ID);
        TempData["InfoBanner"] = "Account verified";
        return Redirect("/logout");
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

        if (!user.Verified)
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
        
        var user = await _userService.FindByEmail(fp.Email);
        if (user == null || !user.Verified)
        {
            ViewBag.ErrorMessage = "Invalid email";
            return View();
        }
        
        bool success = await _mailService.SendPasswortResetMail(user);
        if (success)
        {
            TempData["InfoBanner"] = "Send email to " + user.Email;
        }
        else
        {
            TempData["InfoBanner"] = "Email could not be send. Please try again later!";
        }
        return Redirect("/login");
    }
    
    [AllowAnonymous]
    [Route("changepassword/{token?}")]
    public async Task<IActionResult> ChangePassword(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            ViewBag.Token = token;
            var userToken = await _userService.GetToken(token);
            if (userToken == null) return Redirect("/login");
            
            var expiry = userToken.TokenExpiry;
            if (expiry < DateTime.Now)
            {
                await _userService.RemoveToken(userToken.ID);
                TempData["InfoBanner"] = "Token expired";
                return Redirect("/login");
            }
            
            return View(new ChangePasswordViewModel()
            {
                UseUserToken = true
            });
        }
        else if (HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            return View();
        }
        return NotFound();
        
    }
    
    [AllowAnonymous]
    [HttpPost("changepassword/{token=}")]
    public async Task<IActionResult> ChangePassword([FromForm]ChangePasswordViewModel changePassword)
    {
        if (!ModelState.IsValid)
            return View();

        User? user;
        UserToken? token = null;
        if (!changePassword.UseUserToken)
        {
            var contextUser = HttpContext.User;
            user = await _userService.GetUserByClaim(contextUser);
            if(user == null) return Redirect("/logout");
            
            
            var passwordResult = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, changePassword.OldPassword);
            if (passwordResult != PasswordVerificationResult.Success)
            {
                ViewBag.ErrorMessage = "Wrong password";
                return View();
            }
        }
        else
        {
            token = await _userService.GetToken(changePassword.Token);
            if(token == null) return Redirect("/login");
            
            var expiry = token.TokenExpiry;
            if (expiry < DateTime.Now)
            {
                await _userService.RemoveToken(token.ID);
                TempData["InfoBanner"] = "Token expired";
                return Redirect("/login");
            }
            
            user = token.User;
        }

        if (changePassword.NewPassword != changePassword.ConfirmPassword)
        {
            ViewBag.ErrorMessage = "Passwords do not match";
            return View();
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

        if (token != null)
        {
            await _userService.RemoveToken(token.ID);
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
        if(user == null) return Redirect("/logout");

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
        if(user == null) return Redirect("/logout");

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
        
        bool success = await _mailService.SendChangeAdressMail(user, changeEmail.Email);
        if (success)
        {
            TempData["InfoBanner"] = "Send email to " + changeEmail.Email + ". Please verify!";
        }
        else
        {
            TempData["InfoBanner"] = "Email could not be send. Please try again later!";
        }
        return Redirect("/account");
    }

    [HttpGet("/validatechangemail/{token}")]
    public async Task<IActionResult> ValidateChangeEmail(string token, string newmail, string hash)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newmail) || string.IsNullOrEmpty(hash))
            return Redirect("/");
        
        var t = await _userService.GetToken(token);
        if(t == null) return Redirect("/login");

        if (t.TokenExpiry < DateTime.Now)
        {
            TempData["InfoBanner"] = "Token is expired";
            return Redirect("/");
        }
        var user = t.User;
        
        if (hash != Hasher.UrlSecureHashValue(newmail)) return Redirect("/");
        
        var possibleUser = await _userService.FindByEmail(newmail);
        if (possibleUser != null) return Redirect("/");
        
        user.Email = newmail;

        if (!await _userService.Update(user))
        {
            TempData["InfoBanner"] = "Email could not be changed";
            return Redirect("/");
        }

        await _userService.RemoveToken(t.ID);
        TempData["InfoBanner"] = "Your email was changed to " + newmail;
        return Redirect("/");
        
    }
    

    [Route("deleteaccount")]
    public async Task<IActionResult> DeleteAccount()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("/logout");

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
        if(user == null) return Redirect("/logout");
        
        return View(user);
    }
}