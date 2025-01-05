using IOweYou.Web.Services;
using IOweYou.Web.Services.Account;
using IOweYou.Web.Services.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class SearchController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;

    public SearchController(ILogger<HomeController> logger, IUserService userService, ITransactionService transactionService)
    {
        _logger = logger;
        _userService = userService;
        _transactionService = transactionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Usernames(string searchTerm)
    {
        var users = await _userService.FindUsernames(searchTerm);
        return Json(users);
    }
}