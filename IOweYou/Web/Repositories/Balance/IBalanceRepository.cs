using IOweYou.Models;

namespace IOweYou.Web.Repositories.Balance;

public interface IBalanceRepository : IDbManagement<Models.Transactions.Balance>
{
    
    Task<Models.Transactions.Balance?> GetBalanceByTransaction(Models.Transactions.Transaction transaction);

    Task<Models.Transactions.Balance?> GetBalanceByUsersAndCurr(Models.User user, Models.User partner,
        Models.Transactions.Currency currency);
    Task<List<IGrouping<Models.User, Models.Transactions.Balance>>> GetBalancesFromUserGrouped(Models.User user, bool excludeZeros);
    Task<List<Models.Transactions.Balance>> GetBalancesFromUser(Models.User user, bool excludeZeros);
    Task<List<Models.Transactions.Balance>> GetBalancesToUser(Models.User fromUser, Models.User toUser, bool excludeZeros);
}