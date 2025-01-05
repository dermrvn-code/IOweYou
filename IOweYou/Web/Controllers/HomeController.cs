using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IOweYou.Models;
using IOweYou.ViewModels.Home;
using IOweYou.Web.Services;
using IOweYou.Web.Services.Account;
using IOweYou.Web.Services.Transaction;

namespace IOweYou.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;

    public HomeController(ILogger<HomeController> logger, IUserService userService, ITransactionService transactionService)
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
    }
    
    [Route("/dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        return await LoadViewWithUser();
    }
    
    [Route("/transactions")]
    [HttpGet]
    public async Task<IActionResult> Transactions()
    {
        return await LoadViewWithUser();
    }
    
    [HttpGet("/send")]
    public IActionResult Send()
    {
        return View();
    }
    
    [HttpPost("/send")]
    public async Task<IActionResult> Send([FromForm] SendViewModel send)
    {
        /*int senderId = (int)TempData["UserId"];
        var sender = _context.Users.Find(senderId);
        var receiver = _context.Users.FirstOrDefault(u => u.Username == recipient);

        if (receiver == null)
        {
            ViewBag.Error = "User not found.";
            return View();
        }

        if (sender.Balance >= amount)
        {
            sender.Balance -= amount;
            receiver.Balance += amount;

            var transaction = new Transaction
            {
                SenderId = senderId,
                ReceiverId = receiver.Id,
                Amount = amount
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
        ViewBag.Error = "Insufficient balance.";*/
        return View();
    }
    
    private async Task<IActionResult> LoadViewWithUser()
    {
        
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);

        if (user == null)
            return Redirect("/logout");
        
        return View(user);
    } 

}