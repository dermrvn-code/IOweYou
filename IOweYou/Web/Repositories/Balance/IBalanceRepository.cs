using IOweYou.Models;

namespace IOweYou.Web.Repositories.Balance;

public interface IBalanceRepository : IDbManagement<Models.Transactions.Balance>
{
    
    Task<Models.Transactions.Balance?> GetBalanceByTransaction(Models.Transactions.Transaction transaction);
    Task<List<IGrouping<User, Models.Transactions.Balance>>> GetBalancesFromUser(User user, bool excludeZeros);
}