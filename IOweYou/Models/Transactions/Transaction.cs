using IOweYou.Models.Transactions;

namespace IOweYou.Models;

public class Transaction : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public bool Received { get; set; }
    
    public Guid PartnerId { get; set; }
    public User Partner { get; set; }
    
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}