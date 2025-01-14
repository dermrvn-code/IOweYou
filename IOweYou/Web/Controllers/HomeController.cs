using System.Diagnostics;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using IOweYou.Models;
using IOweYou.Models.Transactions;
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
    private readonly ITransactionService _transactionService;
    private readonly IBalanceService _balanceService;

    public HomeController(ILogger<HomeController> logger, 
        IUserService userService,
        ITransactionService transactionService,
        IBalanceService balanceService
        )
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
        _balanceService = balanceService;
    }
    
    [Route("/dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
            
        if(user == null) return Redirect("logout");

        var transactions = await _transactionService.GetTransactionsFromUser(user);
        var balances = await _balanceService.GetBalancesFromUser(user, excludeZeros: true);

        ViewData["showUsernamesInBalances"] = true;
        return View(new DashbordViewModel()
        {
            User = user,
            Transactions = transactions.Take(3).ToList(),
            Balances = balances.Take(3).ToList()
        });
    }

}