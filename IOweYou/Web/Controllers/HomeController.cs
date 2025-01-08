using System.Diagnostics;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using IOweYou.Models;
using IOweYou.ViewModels.Home;
using IOweYou.Web.Services;
using IOweYou.Web.Services.Account;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using Transaction = IOweYou.Models.Transactions.Transaction;

namespace IOweYou.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;
    private readonly ICurrencyService _currencyService;

    public HomeController(ILogger<HomeController> logger, IUserService userService, ITransactionService transactionService, ICurrencyService currencyService)
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
        _currencyService = currencyService;
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
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);

        if (user == null)
            return Redirect("logout");

        var transactions = await _transactionService.GetTransactionsFromUserID(user.ID);
        return View(transactions);
    }
    
    [HttpGet("/send")]
    public async Task<IActionResult> Send()
    {
        ViewBag.CurrencyList = await _currencyService.GetAll();;
        return View();
    }

    [HttpPost("/send")]
    public async Task<IActionResult> Send([FromForm] SendViewModel send)
    {
        if (ModelState.IsValid){
            User partner = await _userService.FindByUsername(send.UserToSendTo);
            if (partner == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return View();
            }

            if (send.Value <= 0)
            {
                ViewBag.ErrorMessage = "Please enter a value";
                return View();
            }

            var currency = await _currencyService.GetByName(send.Currency);
            if (currency == null)
            {
                ViewBag.ErrorMessage = "Currency not found";
                return View();
            }
            
            var contextUser = HttpContext.User;
            var thisUser = await _userService.GetUserByClaim(contextUser);

            if (thisUser == null) return Redirect("logout");
            
            var myTransaction = new Transaction(thisUser, partner, currency, (decimal)send.Value, false);
            var partnerTransaction = myTransaction.Invert();
            
            thisUser.Transactions.Add(myTransaction);
            partner.ExternalTransactions.Add(partnerTransaction);
            
            var success = await _userService.Update(thisUser) && await _userService.Update(partner);
            if (!success)
            {
                ViewBag.ErrorMessage = "Problem with transaction";
                return View();
            }
        }

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