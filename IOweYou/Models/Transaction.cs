namespace IOweYou.Models;

public class Transaction : Entity
{
    public User User { get; set; }
    public bool Received { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}