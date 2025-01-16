namespace IOweYou.Web.Repositories;

public interface IDbManagement<T>
{
    public Task<List<T>> GetAll();
    public Task<T?> GetSingle(Guid id);
    public Task<bool> Add(T entity);
    public Task<bool> Delete(Guid id);
    public Task<bool> Update(T entity);
}