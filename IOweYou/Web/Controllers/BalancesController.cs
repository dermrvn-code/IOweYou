using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class BalancesController : Controller
{
    private readonly IBalanceService _balanceService;
    private readonly ILogger<BalancesController> _logger;
    private readonly IUserService _userService;

    public BalancesController(ILogger<BalancesController> logger,
        IUserService userService,
        ITransactionService transactionService,
        ICurrencyService currencyService,
        IBalanceService balanceService
    )
    {
        _logger = logger;
        _userService = userService;
        _balanceService = balanceService;
    }


    [Route("/balances")]
    public async Task<IActionResult> Balances()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if (user == null) return Redirect("/logout");


        var balances = await _balanceService.GetBalancesFromUserGrouped(user, true);
        return View(balances);
    }

    [Route("/balances/{username?}")]
    public async Task<IActionResult> UserBalances(string? username)
    {
        if (string.IsNullOrEmpty(username)) return NotFound();

        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if (user == null) return Redirect("/logout");

        var partner = await _userService.FindByUsername(username);
        if (partner == null) return NotFound();

        ViewBag.Partner = partner;
        var balances = await _balanceService.GetBalancesToUser(user, partner, true);
        return View(balances);
    }
}