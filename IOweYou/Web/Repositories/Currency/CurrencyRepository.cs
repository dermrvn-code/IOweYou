using IOweYou.Database;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.Currency;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly DatabaseContext _context;

    private readonly ILogger<CurrencyRepository> _logger;

    private readonly List<(string name, decimal unit)> currencies = new()
    {
        ("Coffee", 1),
        ("Softdrink", 3),
        ("Sandwich", 3),
        ("Fries", 3),
        ("Beer", 4),
        ("Burger", 9),
        ("Cocktail", 9),
        ("Kebab", 7),
        ("Pizza", 10)
    };

    public CurrencyRepository(ILogger<CurrencyRepository> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task SyncCurrencies()
    {
        foreach (var currency in currencies)
        {
            var existingCurrency = await GetByName(currency.name);

            if (existingCurrency == null)
            {
                await Add(new Models.Transactions.Currency
                {
                    Name = currency.name,
                    UnitValue = currency.unit
                });
            }
            else if (existingCurrency.UnitValue != currency.unit)
            {
                existingCurrency.UnitValue = currency.unit;
                await Update(existingCurrency);
            }
        }

        var currencyNamesToKeep = currencies.Select(c => c.name).ToList();

        var currenciesToDelete = _context.Currencies
            .Where(c => !currencyNamesToKeep.Contains(c.Name))
            .ToList();

        if (currenciesToDelete.Any()) _context.Currencies.RemoveRange(currenciesToDelete);

        await _context.SaveChangesAsync();
    }


    public async Task<List<Models.Transactions.Currency>> GetAll()
    {
        return await _context.Currencies.ToListAsync();
    }

    public async Task<Models.Transactions.Currency?> GetSingle(Guid id)
    {
        return await _context.Currencies.FindAsync(id);
    }

    public async Task<bool> Add(Models.Transactions.Currency entity)
    {
        var user = await _context.Currencies.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var transaction = await _context.Currencies.FindAsync(id);
        if (transaction == null) return false;

        _context.Currencies.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Models.Transactions.Currency entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Models.Transactions.Currency?> GetByName(string name)
    {
        return await _context.Currencies.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<List<string>> FindCurrencies(string name)
    {
        if (string.IsNullOrEmpty(name)) return await _context.Currencies.Select(c => c.Name).ToListAsync();
        return await _context.Currencies.Where(c => c.Name.Contains(name))
            .Select(c => c.Name).ToListAsync();
    }
}