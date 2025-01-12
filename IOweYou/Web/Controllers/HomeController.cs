using System.Diagnostics;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using IOweYou.Models;
using IOweYou.ViewModels.Home;
using IOweYou.Web.Services;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Transaction = IOweYou.Models.Transactions.Transaction;

namespace IOweYou.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, 
        IUserService userService
        )
    {
        _logger = logger;
        _userService = userService;
    }
    
    [Route("/dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
            
        if(user == null) return Redirect("logout");
        
        return View(user);
    }

}