using IOweYou.Database;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.Balance;

public class BalanceRepository : IBalanceRepository
{
   
    private readonly DatabaseContext _context;

    public BalanceRepository(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<List<Models.Transactions.Balance>> GetAll()
    {
        return await _context.Balances.ToListAsync();
    }

    public async Task<Models.Transactions.Balance?> GetSingle(Guid id)
    {
        return await _context.Balances.FindAsync(id);
    }

    public async Task<bool> Add(Models.Transactions.Balance entity)
    {
        var user = await _context.Balances.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var transaction = await _context.Balances.FindAsync(id);
        if (transaction == null) return false;
        
        _context.Balances.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Models.Transactions.Balance entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }
    
}