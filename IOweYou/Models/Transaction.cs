namespace IOweYou.Models;

public class Transaction : Entity
{
    public User User { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}