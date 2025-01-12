using IOweYou.Models;

namespace IOweYou.Web.Repositories.Transaction;

public interface ITransactionRepository : IDbManagement<Models.Transactions.Transaction>
{
    
    Task<List<Models.Transactions.Transaction>> GetTransactionsFromUserId(Guid userId);
    Task<List<Models.Transactions.Transaction>> GetTransactionsWithUser(User user, User partner);
    Task<bool> CreateTransaction(User user, User partner, Models.Transactions.Currency currency, decimal amount);
}