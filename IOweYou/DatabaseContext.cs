using IOweYou.Models;
using Microsoft.AspNetCore.Mvc;

namespace IOweYou;

public class DatabaseContext : ControllerBase
{
    private readonly UserContext _context;

    public DatabaseContext(UserContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            await _context.Database.CanConnectAsync();
            return Ok("Database connection successful");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Database connection failed: {ex.Message}");
        }
    }
}