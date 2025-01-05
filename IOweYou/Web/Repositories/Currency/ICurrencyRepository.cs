namespace IOweYou.Web.Repositories.Currency;

public interface ICurrencyRepository : IDbManagement<Models.Transactions.Currency>
{
    Task SyncCurrencies();
    Task<Models.Transactions.Currency?> GetByName(string name);
    Task<List<string>> FindCurrencies(string name);
}