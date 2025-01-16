using IOweYou.ViewModels.Transactions;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class TransactionsController : Controller
{
    private readonly IBalanceService _balanceService;
    private readonly ICurrencyService _currencyService;
    private readonly ILogger<TransactionsController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly IUserService _userService;

    public TransactionsController(ILogger<TransactionsController> logger,
        IUserService userService,
        ITransactionService transactionService,
        ICurrencyService currencyService,
        IBalanceService balanceService
    )
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
        _currencyService = currencyService;
        _balanceService = balanceService;
    }


    [Route("/transactions")]
    public async Task<IActionResult> Transactions()
    {
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);

        if (user == null) return Redirect("/logout");

        var transactions = await _transactionService.GetTransactionsFromUser(user);
        return View(transactions);
    }


    [Route("/transactions/{username?}")]
    public async Task<IActionResult> UserTransactions(string? username)
    {
        if (string.IsNullOrEmpty(username)) return NotFound();
        var contextUser = HttpContext.User;
        var user = await _userService.GetUserByClaim(contextUser);
        if (user == null) return Redirect("/logout");

        var partner = await _userService.FindByUsername(username);
        if (partner == null) return NotFound();

        ViewBag.Partner = partner;
        var transactions = await _transactionService.GetTransactionsWithUser(user, partner);
        return View(transactions);
    }

    [Route("/send/{username?}")]
    public async Task<IActionResult> Send(string? username)
    {
        ViewBag.CurrencyList = await _currencyService.GetAll();

        if (!string.IsNullOrEmpty(username))
        {
            var user = await _userService.FindByUsername(username);

            if (user != null)
                return View(new TransactionViewModel
                {
                    UserToSendTo = username
                });
        }

        return View();
    }

    private async Task<IActionResult> HandleTransaction(TransactionViewModel transaction, bool resolve)
    {
        if (ModelState.IsValid)
        {
            var partner = await _userService.FindByUsername(transaction.UserToSendTo);
            if (partner == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return View();
            }

            if (transaction.Value <= 0)
            {
                ViewBag.ErrorMessage = "Please enter a value";
                return View();
            }

            var currency = await _currencyService.GetByName(transaction.Currency);
            if (currency == null)
            {
                ViewBag.ErrorMessage = "Currency not found";
                return View();
            }

            var contextUser = HttpContext.User;
            var thisUser = await _userService.GetUserByClaim(contextUser);

            if (thisUser == null) return Redirect("/logout");

            if (thisUser.ID == partner.ID) ViewBag.ErrorMessage = "You cannot send yourself";

            var success =
                await _transactionService.CreateTransaction(thisUser, partner, currency, (decimal)transaction.Value,
                    resolve);
            if (!success)
            {
                ViewBag.ErrorMessage = "Problem with transaction";
                return View();
            }

            if (resolve)
                TempData["InfoBanner"] = "Resolved " + transaction.Value + " " + transaction.Currency + " with " +
                                         partner.Username;
            else
                TempData["InfoBanner"] =
                    "Send " + transaction.Value + " " + transaction.Currency + " to " + partner.Username;

            return Redirect("/");
        }

        return View();
    }

    [HttpPost("/send")]
    public async Task<IActionResult> Send([FromForm] TransactionViewModel transaction)
    {
        ViewBag.CurrencyList = await _currencyService.GetAll();
        return await HandleTransaction(transaction, false);
    }

    [Route("/resolve/{id}")]
    public async Task<IActionResult> Resolve(string id)
    {
        if (string.IsNullOrEmpty(id)) return NotFound();

        var balance = await _balanceService.GetSingle(new Guid(id));
        if (balance == null) return NotFound();

        if (balance.Amount < 0) return NotFound();

        var toUser = await _userService.GetSingle(balance.ToUserId);
        if (toUser == null) return NotFound();

        var currency = await _currencyService.GetSingle(balance.CurrencyId);
        if (currency == null) return NotFound();

        return View(new TransactionViewModel
        {
            UserToSendTo = balance.ToUser.Username,
            Value = (double)balance.Amount,
            Currency = balance.Currency.Name
        });
    }

    [HttpPost("/resolve/{id}")]
    public async Task<IActionResult> Resolve([FromForm] TransactionViewModel transaction)
    {
        ViewBag.CurrencyList = await _currencyService.GetAll();
        return await HandleTransaction(transaction, true);
    }
}