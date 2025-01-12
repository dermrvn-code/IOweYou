using IOweYou.ViewModels.User;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Currency;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class UserController : Controller
{
    
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly ITransactionService _transactionService;
    private readonly IBalanceService _balanceService;

    public UserController(ILogger<UserController> logger, 
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
    
    
    [Route("/user/{username?}")]
    public async Task<IActionResult> User(string? username)
    {   
        var contextUser = HttpContext.User;
        var thisUser = await _userService.GetUserByClaim(contextUser);
        if(thisUser == null) return Redirect("logout");
        
        if(thisUser.Username == username) return Redirect("/account");
        
        if (username == null) return BadRequest();
        var user = await _userService.FindByUsername(username);
        if(user == null) return NotFound();

        var transactions = await _transactionService.GetTransactionsWithUser(thisUser, user);
        var balances = await _balanceService.GetBalancesToUser(thisUser, user, excludeZeros: true);

        
        //var transactions = await _transactionService.GetTransactionsWithUser(user, partner);
        return View(new UserPageViewModel()
        {
            User = user,
            Transactions = transactions.Take(3).ToList(),
            Balances = balances.Take(3).ToList()
        });
    }

    [HttpGet("/searchUser")]
    public async Task<IActionResult> SearchUser(string username)
    {
        var user = await _userService.FindByUsername(username);
        if(user == null) return NotFound();

        return Redirect("/user/" + user.Username);
    }
}