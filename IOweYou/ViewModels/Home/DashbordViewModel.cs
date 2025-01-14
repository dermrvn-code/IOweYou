using IOweYou.Models.Transactions;

namespace IOweYou.ViewModels.Home;

public class DashbordViewModel
{
    
    public Models.User User { get; set; }
    public List<Balance> Balances { get; set; }
    public List<Transaction> Transactions { get; set; }
}