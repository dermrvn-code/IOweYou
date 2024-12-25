using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IOweYou.Models;

namespace IOweYou.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    [Route("/dashboard")]
    public IActionResult Dashboard()
    {
        //int userId = (int)TempData["UserId"];
        //var user = _context.Users.Find(userId);
        //var user = new User();
        //user.Username = "User";
        //return View(user);
        return View();
    }
    
    [Route("/transactions")]
    [HttpGet]
    public IActionResult Transactions()
    {
        return View();
    }
    
    [Route("/send")]
    [HttpGet]
    public IActionResult Send()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Send(string recipient, decimal amount)
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
}