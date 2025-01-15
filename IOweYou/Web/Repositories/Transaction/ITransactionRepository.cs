using IOweYou.Models;

namespace IOweYou.Web.Repositories.Transaction;

public interface ITransactionRepository : IDbManagement<Models.Transactions.Transaction>
{
    
    Task<List<Models.Transactions.Transaction>> GetTransactionsFromUser(Models.User user);
    Task<List<Models.Transactions.Transaction>> GetTransactionsWithUser(Models.User user, Models.User partner);
    Task<bool> CreateTransaction(Models.User user, Models.User partner, Models.Transactions.Currency currency, decimal amount, bool resolve);
}