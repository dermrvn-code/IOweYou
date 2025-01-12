using IOweYou.Models;
using IOweYou.Models.Transactions;

namespace IOweYou.ViewModels.Balances;

public class UserBalancesViewModel
{
    public User FromUser { get; set; }
    public User ToUser { get; set; }
    public List<Balance> Balances { get; set; }
}