using IOweYou.Web.Services.APIs;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.Transaction;
using IOweYou.Web.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou.Web.Controllers;

public class APIsController : Controller
{
    
    private readonly ILogger<APIsController> _logger;
    private readonly IUserService _userService;
    private readonly IQrCodeService _qrCodeService;

    public APIsController(ILogger<APIsController> logger, 
        IUserService userService,
        IQrCodeService qrCodeService
    )
    {
        _logger = logger;
        _userService = userService;
        _qrCodeService = qrCodeService;
    }
    
    [Route("api/qr/{username?}")]
    public async Task<IActionResult> QRCode(string? username)
    {
        if(username == null) return BadRequest();
        
        var user = await _userService.FindByUsername(username);
        if(user == null) return NotFound();

        var qr = await _qrCodeService.GetQrCodeForUser(user);
        if(qr == null) return NotFound();
        
        return Ok(qr);

    }
}