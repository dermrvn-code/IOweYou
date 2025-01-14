using System.Diagnostics;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using IOweYou.Models;
using IOweYou.ViewModels.Transactions;
using IOweYou.Web.Services;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Transaction = IOweYou.Models.Transactions.Transaction;

namespace IOweYou.Web.Controllers;

public class TransactionsController : Controller
{
    private readonly ILogger<TransactionsController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;
    private readonly ICurrencyService _currencyService;

    public TransactionsController(ILogger<TransactionsController> logger, 
        IUserService userService, 
        ITransactionService transactionService, 
        ICurrencyService currencyService
        )
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
        _currencyService = currencyService;
    }
    
    
    [Route("/transactions")]
    public async Task<IActionResult> Transactions()
    {
        
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
            
        if(user == null) return Redirect("logout");

        var transactions = await _transactionService.GetTransactionsFromUser(user);
        return View(transactions);
    }
    
    
    [Route("/transactions/{id?}")]
    public async Task<IActionResult> UserTransactions(Guid? id)
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if(user == null) return Redirect("logout");
        
        var partner = await _userService.GetSingle(id.GetValueOrDefault());
        if(partner == null) return NotFound();
        
        ViewBag.Partner = partner;
        var transactions = await _transactionService.GetTransactionsWithUser(user, partner);
        return View(transactions);
    }
    
    [Route("/send/{username?}")]
    public async Task<IActionResult> Send(string? username)
    {
        ViewBag.CurrencyList = await _currencyService.GetAll();

        if (!String.IsNullOrEmpty(username))
        {
            var user = await _userService.FindByUsername(username);

            if (user != null)
            {
                return View(new SendViewModel()
                {
                    UserToSendTo = username
                });
            }
        }

        return View();
    }

    [HttpPost("/send")]
    public async Task<IActionResult> Send([FromForm] SendViewModel send)
    {
        ViewBag.CurrencyList = await _currencyService.GetAll();
        
        if (ModelState.IsValid){
            User? partner = await _userService.FindByUsername(send.UserToSendTo);
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

            if (thisUser.ID == partner.ID)
            {
                ViewBag.ErrorMessage = "You cannot send yourself";
            }

            var success = await _transactionService.CreateTransaction(thisUser, partner, currency, (decimal)send.Value);
            if (!success)
            {
                ViewBag.ErrorMessage = "Problem with transaction";
                return View();
            }
            
            // TODO find a better solution to this
            TempData["InfoBanner"] = "Send " + send.Value + " " + send.Currency + " to " + partner.Username;
            return Redirect("/");
        }

        return View();
    }

}