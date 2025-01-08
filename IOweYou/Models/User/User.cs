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
    
    public List<Transaction> Transactions { get; set; }
    public List<Transaction> ExternalTransactions { get; set; }
    public List<Balance> FromBalances { get; set; }
    public List<Balance> ToBalances { get; set; }

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
    }
}

