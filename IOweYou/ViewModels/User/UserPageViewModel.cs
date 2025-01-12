using IOweYou.Models.Transactions;

namespace IOweYou.ViewModels.User;

public class UserPageViewModel
{
    public Models.User User { get; set; }
    public List<Transaction> Transactions { get; set; }
    public List<Balance> Balances { get; set; }

}