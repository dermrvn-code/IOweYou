using IOweYou.Database;
using IOweYou.Web.Services.Balance;
using IOweYou.Web.Services.User;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.Transaction;

public class TransactionRepository : ITransactionRepository
{
    private readonly IBalanceService _balanceService;

    private readonly DatabaseContext _context;
    private readonly IUserService _userService;

    public TransactionRepository(DatabaseContext context, IUserService userService, IBalanceService balanceService)
    {
        _context = context;
        _userService = userService;
        _balanceService = balanceService;
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

    public async Task<List<Models.Transactions.Transaction>> GetTransactionsFromUser(Models.User user)
    {
        return await _context.Transactions.Where(
                t => t.UserId == user.ID)
            .Include(t => t.User)
            .Include(t => t.Partner)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<List<Models.Transactions.Transaction>> GetTransactionsWithUser(Models.User user,
        Models.User partner)
    {
        return await _context.Transactions.Where(
                t => t.UserId == user.ID
                     && t.PartnerId == partner.ID)
            .Include(t => t.User)
            .Include(t => t.Partner)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<bool> CreateTransaction(Models.User user, Models.User partner,
        Models.Transactions.Currency currency, decimal amount, bool resolve)
    {
        var myTransaction = new Models.Transactions.Transaction(user, partner, currency, amount, false, resolve);
        var partnerTransaction = myTransaction.Invert();

        user.Transactions.Add(myTransaction);
        partner.Transactions.Add(partnerTransaction);


        var partnerBalance = await _balanceService.GetBalanceByTransaction(partnerTransaction);
        if (partnerBalance == null)
        {
            partnerBalance = new Models.Transactions.Balance(partner, user, currency, amount);
            await _balanceService.Add(partnerBalance);
        }
        else
        {
            partnerBalance.Amount = partnerBalance.Amount + amount;
            await _balanceService.Update(partnerBalance);
        }

        var myBalance = await _balanceService.GetBalanceByTransaction(myTransaction);
        if (myBalance == null)
        {
            myBalance = new Models.Transactions.Balance(user, partner, currency, -amount);
            await _balanceService.Add(myBalance);
        }
        else
        {
            myBalance.Amount = myBalance.Amount - amount;
            await _balanceService.Update(myBalance);
        }


        return
            await _userService.Update(user) &&
            await _userService.Update(partner);
    }
}