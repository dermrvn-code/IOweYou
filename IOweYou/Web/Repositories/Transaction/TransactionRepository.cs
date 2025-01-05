using IOweYou.Database;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.Transaction;

public class TransactionRepository : ITransactionRepository
{
   
    private readonly DatabaseContext _context;

    public TransactionRepository(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<List<Models.Transactions.Transaction>> GetAll()
    {
        return await _context.Transactions.ToListAsync();
    }

    public async Task<Models.Transactions.Transaction?> GetSingle(Guid id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task<bool> Add(Models.Transactions.Transaction entity)
    {
        var user = await _context.Transactions.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return false;
        
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Models.Transactions.Transaction entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }
}