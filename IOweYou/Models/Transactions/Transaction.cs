using IOweYou.Models.Transactions;

namespace IOweYou.Models.Transactions;

public class Transaction : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public bool Received { get; set; }
    
    public Guid PartnerId { get; set; }
    public User Partner { get; set; }
    
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    public Transaction() {}

    public Transaction(User user, User partner, Currency currency, decimal amount, bool received)
    {
        User = user;
        UserId = user.ID;
        Partner = partner;
        PartnerId = partner.ID;
        Currency = currency;
        Amount = amount;
        Received = received;
    }
    
    public Transaction Invert()
    {
        return new Transaction(
            user: Partner, 
            partner: User, 
            currency: Currency, 
            amount: Amount,
            received: !this.Received
        );
    }
}