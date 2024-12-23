using IOweYou.Models;

namespace IOweYou.Web.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
    User GetSingle(Guid id);
    User Add(User entity);
    void Delete(Guid id);
    User Update(User entity);
    User FindByUsername(string username);
    User FindByEmail(string email);
    User FindByLogin(string login, string password);
    
    
}