using IOweYou.Web.Repositories.Transaction;

namespace IOweYou.Web.Services.Transaction;

public class TransactionService : ITransactionService
{
    
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<List<Models.Transactions.Transaction>> GetAll()
    {
        return await _transactionRepository.GetAll();
    }

    public async Task<Models.Transactions.Transaction?> GetSingle(Guid id)
    {
        return await _transactionRepository.GetSingle(id);
    }

    public async Task<bool> Add(Models.Transactions.Transaction entity)
    {
        return await _transactionRepository.Add(entity);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _transactionRepository.Delete(id);
    }

    public async Task<bool> Update(Models.Transactions.Transaction entity)
    {
        return await _transactionRepository.Update(entity);
    }

    public async Task<List<Models.Transactions.Transaction>> GetTransactionsFromUserID(Guid userId)
    {
        return await _transactionRepository.GetTransactionsFromUserID(userId);
    }

    
}