using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using IOweYou.Models.Transactions;

namespace IOweYou.Models;

public class User : Entity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateCreated { get; set; }
    public List<UserToken> Tokens { get; set; } = new List<UserToken>();
    
    public bool Verified { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    public List<Balance> Balances { get; set; } = new List<Balance>();

    public List<Claim> ToClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, Email), new Claim("ID", ID.ToString())
        };

        return claims;
    }

    public User(string username, string email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Verified = false;
        DateCreated = DateTime.Now;
    }
}

