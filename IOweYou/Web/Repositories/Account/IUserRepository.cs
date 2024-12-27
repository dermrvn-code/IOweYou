using System.Security.Claims;
using IOweYou.Models;

namespace IOweYou.Web.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> GetSingle(Guid id);
    Task<bool> Add(User entity);
    Task<bool> Delete(Guid id);
    Task<bool> Update(User entity);
    Task<User?> FindByUsername(string username);
    Task<User?> FindByEmail(string email);
    Task<User?> FindByLogin(string login, string passwordHash);
    Task<User?> GetUserByClaim(ClaimsPrincipal claim);


}