using IOweYou.Web.Repositories.Currency;

namespace IOweYou.Web.Services.Currency;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task SyncCurrencies()
    {
        await _currencyRepository.SyncCurrencies();
    }

    public async Task<List<Models.Transactions.Currency>> GetAll()
    {
        return await _currencyRepository.GetAll();
    }

    public async Task<Models.Transactions.Currency?> GetSingle(Guid id)
    {
        return await _currencyRepository.GetSingle(id);
    }

    public async Task<bool> Add(Models.Transactions.Currency entity)
    {
        return await _currencyRepository.Add(entity);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _currencyRepository.Delete(id);
    }

    public async Task<bool> Update(Models.Transactions.Currency entity)
    {
        return await _currencyRepository.Update(entity);
    }

    public async Task<Models.Transactions.Currency?> GetByName(string name)
    {
        return await _currencyRepository.GetByName(name);
    }

    public async Task<List<string>> FindCurrencies(string name)
    {
        return await _currencyRepository.FindCurrencies(name);
    }
}