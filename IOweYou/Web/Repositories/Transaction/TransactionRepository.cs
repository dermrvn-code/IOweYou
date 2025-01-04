using IOweYou.Database;

namespace IOweYou.Web.Repositories.Transaction;

public class TransactionRepository : ITransactionRepository
{
    
    private readonly DatabaseContext _context;

    public TransactionRepository(DatabaseContext context)
    {
        _context = context;
    }
    
}