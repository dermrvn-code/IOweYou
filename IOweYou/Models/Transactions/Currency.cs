namespace IOweYou.Models.Transactions;

public class Currency : Entity
{
    public string Name { get; set; }
    public decimal UnitValue { get; set; }
}