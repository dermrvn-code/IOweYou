using IOweYou.Models;

namespace IOweYou.Web.Repositories.Transaction;

public interface ITransactionRepository : IDbManagement<Models.Transactions.Transaction>
{
    
    Task<List<Models.Transactions.Transaction>> GetTransactionsFromUserId(Guid userId);
    Task<bool> CreateTransaction(User user, User partner, Models.Transactions.Currency currency, decimal amount);
}