using IOweYou.Web.Repositories.Balance;

namespace IOweYou.Web.Services.Balance;

public class BalanceService : IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;

    public BalanceService(IBalanceRepository balanceRepository)
    {
        _balanceRepository = balanceRepository;
    }

    public async Task<List<Models.Transactions.Balance>> GetAll()
    {
        return await _balanceRepository.GetAll();
    }

    public async Task<Models.Transactions.Balance?> GetSingle(Guid id)
    {
        return await _balanceRepository.GetSingle(id);
    }

    public async Task<bool> Add(Models.Transactions.Balance entity)
    {
        return await _balanceRepository.Add(entity);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _balanceRepository.Delete(id);
    }

    public async Task<bool> Update(Models.Transactions.Balance entity)
    {
        return await _balanceRepository.Update(entity);
    }

    public async Task<Models.Transactions.Balance?> GetBalanceByTransaction(Models.Transactions.Transaction transaction)
    {
        return await _balanceRepository.GetBalanceByTransaction(transaction);
    }

    public async Task<Models.Transactions.Balance?> GetBalanceByUsersAndCurr(Models.User user, Models.User partner,
        Models.Transactions.Currency currency)
    {
        return await _balanceRepository.GetBalanceByUsersAndCurr(user, partner, currency);
    }

    public async Task<List<IGrouping<Models.User, Models.Transactions.Balance>>> GetBalancesFromUserGrouped(
        Models.User user, bool excludeZeros)
    {
        return await _balanceRepository.GetBalancesFromUserGrouped(user, excludeZeros);
    }

    public async Task<List<Models.Transactions.Balance>> GetBalancesToUser(Models.User fromUser, Models.User toUser,
        bool excludeZeros)
    {
        return await _balanceRepository.GetBalancesToUser(fromUser, toUser, excludeZeros);
    }

    public async Task<List<Models.Transactions.Balance>> GetBalancesFromUser(Models.User user, bool excludeZeros)
    {
        return await _balanceRepository.GetBalancesFromUser(user, excludeZeros);
    }
}