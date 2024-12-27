using IOweYou.Database;
using IOweYou.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Database;

public class DatabaseController : ControllerBase
{
    private readonly DatabaseContext _context;

    public DatabaseController(DatabaseContext context)
    {
        _context = context;
    }
    
    [AllowAnonymous]
    [HttpGet("check-db")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
            }
            return Ok("Database connection successful");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Database connection failed: {ex.Message}");
        }

    }
}