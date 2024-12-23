using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IOweYou.Models;

public class User : Entity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public Balance Balance { get; set; } = new Balance();

    public List<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Email), new Claim("ID", ID.ToString())
        };

        return claims;
    }
}

