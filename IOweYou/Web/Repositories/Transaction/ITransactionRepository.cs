namespace IOweYou.Web.Repositories.Transaction;

public interface ITransactionRepository : IDbManagement<Models.Transactions.Transaction>
{
    
    Task<List<Models.Transactions.Transaction>> GetTransactionsFromUserID(Guid userId);
}