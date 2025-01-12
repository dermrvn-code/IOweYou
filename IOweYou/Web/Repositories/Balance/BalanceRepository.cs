using IOweYou.Database;
using IOweYou.Models;
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

    public async Task<Models.Transactions.Balance?> GetBalanceByTransaction(Models.Transactions.Transaction transaction)
    {
        return await _context.Balances.Where(
                b => b.CurrencyId == transaction.Currency.ID
                     && b.FromUserId == transaction.User.ID
                     && b.ToUserId == transaction.Partner.ID
                    )
                .Include(b => b.Currency) 
                .Include(b => b.FromUser) 
                .Include(b => b.ToUser)
                .FirstOrDefaultAsync();
    }

    public async Task<List<IGrouping<User, Models.Transactions.Balance>>> GetBalancesFromUser(User user, bool excludeZeros)
    {
        return await _context.Balances
            .AsNoTracking()
            .Where(
                b => b.FromUserId == user.ID && (b.Amount != 0 || !excludeZeros)
            )
            .Include(b => b.Currency)
            .Include(b => b.ToUser)
            .OrderBy(b => b.LastUpdated)
            .GroupBy(b => b.ToUser)
            .ToListAsync();
    }

    public async Task<List<Models.Transactions.Balance>> GetBalancesToUser(User fromUser, User toUser, bool excludeZeros)
    {
        return await _context.Balances
            .AsNoTracking()
            .Where(
                b => b.FromUserId == fromUser.ID 
                     && b.ToUserId == toUser.ID
                     && (b.Amount != 0 || !excludeZeros)
            )
            .Include(b => b.Currency)
            .OrderBy(b => b.LastUpdated)
            .ToListAsync();
    }
    
}