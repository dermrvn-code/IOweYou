using System.Security.Claims;
using IOweYou.Models;
using IOweYou.Web;

namespace IOweYou.Web.Repositories.Account;

public interface IUserRepository : IDbManagement<User>
{
    Task<User?> FindByUsername(string username);
    Task<User?> FindByEmail(string email);
    Task<User?> FindByLogin(string login, string passwordHash);
    Task<User?> GetUserByClaim(ClaimsPrincipal claim);
    Task<List<string>> FindUsernames(string name);


}