using IOweYou.Models;

namespace IOweYou.Web.Repositories;

public class UserRepository : IUserRepository
{

    public IEnumerable<User> GetAll()
    {
        return null;
    }

    public User GetSingle(Guid id)
    {
        return null;
    }

    public User Add(User entity)
    {
        return null;
    }

    public void Delete(Guid id)
    {
    }
    
    public User Update(User entity){
        return null;
        
    }
    
    public User FindByUsername(string username)
    {
        return null;
    }
    
    public User FindByEmail(string email)
    {
        return null;
    }
    
    public User FindByLogin(string login, string password)
    {
        return null;
    }
}