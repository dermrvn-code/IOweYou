using IOweYou.Web.Services;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class SearchController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;
    private readonly ICurrencyService _currencyService;
    private readonly IBalanceService _balanceService;

    public SearchController(ILogger<HomeController> logger, IUserService userService, ITransactionService transactionService, ICurrencyService currencyService, IBalanceService balanceService)
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
        _currencyService = currencyService;
        _balanceService = balanceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Usernames(string searchTerm, bool showMyself)
    {
        Guid userId = Guid.Empty;
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if (user == null)
        {
            showMyself = false;
        }
        else
        {
            userId = user.ID;
        }
        
        
        var users = await _userService.FindUsernames(searchTerm, showMyself, userId);
        return Json(users);
    }
    
    public async Task<IActionResult> Users()
    {
        var users = await _userService.GetAll();
        return Json(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> Currencies(string searchTerm)
    {
        var currencies = await _currencyService.FindCurrencies(searchTerm);
        return Json(currencies);
    }
    
    public async Task<IActionResult> Transactions()
    {
        var transactions = await _transactionService.GetAll();
        return Json(transactions);
    }
    
    [HttpGet]
    public async Task<IActionResult> Balances(string searchTerm)
    {
        var user = await _userService.FindByUsername(searchTerm);
        
        if (user == null)
            return NotFound();
        
        var balances = await _balanceService.GetBalancesFromUser(user, excludeZeros: true);
        return Json(balances);
    }
}