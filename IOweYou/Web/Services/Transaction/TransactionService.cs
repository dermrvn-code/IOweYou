using IOweYou.Web.Repositories.Transaction;

namespace IOweYou.Web.Services.Transaction;

public class TransactionService : ITransactionService
{
    
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    
}