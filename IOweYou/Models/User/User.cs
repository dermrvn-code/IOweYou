using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IOweYou.Models;

public class User : Entity
{
    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    // public Balance Balance { get; set; } = new Balance();
    
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    public List<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Email), new Claim("ID", ID.ToString())
        };

        return claims;
    }
}

