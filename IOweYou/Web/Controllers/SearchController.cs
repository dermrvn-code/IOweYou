using IOweYou.Web.Services;
using IOweYou.Web.Services.Account;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class SearchController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;
    private readonly ICurrencyService _currencyService;

    public SearchController(ILogger<HomeController> logger, IUserService userService, ITransactionService transactionService, ICurrencyService currencyService)
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
        _currencyService = currencyService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Usernames(string searchTerm)
    {
        var users = await _userService.FindUsernames(searchTerm);
        return Json(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> Currencies(string searchTerm)
    {
        var currencies = await _currencyService.FindCurrencies(searchTerm);
        return Json(currencies);
    }
}