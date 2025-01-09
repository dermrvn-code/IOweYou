namespace IOweYou.Web.Repositories.Balance;

public interface IBalanceRepository : IDbManagement<Models.Transactions.Balance>
{
    
    Task<Models.Transactions.Balance?> GetBalanceByTransaction(Models.Transactions.Transaction transaction);
}