using IOweYou.Database;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.Currency;

public class CurrencyRepository : ICurrencyRepository
{
   
    private readonly DatabaseContext _context;

    public CurrencyRepository(DatabaseContext context)
    {
        _context = context;
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
    
}