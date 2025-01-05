using IOweYou.Database;

namespace IOweYou.Web.Repositories;

public interface IDbManagement<T>
{
    public abstract Task<List<T>> GetAll();
    public abstract Task<T?> GetSingle(Guid id);
    public abstract Task<bool> Add(T entity);
    public abstract Task<bool> Delete(Guid id);
    public abstract Task<bool> Update(T entity);
}