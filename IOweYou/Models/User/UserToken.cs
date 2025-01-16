using System.Security.Cryptography;

namespace IOweYou.Models;

public class UserToken : Entity
{
    public Guid UserID { get; set; }
    public User User { get; set; }
    public DateTime TokenExpiry { get; set; }
    
    public UserToken() {}
    
    public UserToken(User user, int hours = 1)
    {
        User = user;
        TokenExpiry = DateTime.Now.AddHours(hours);
    }
}