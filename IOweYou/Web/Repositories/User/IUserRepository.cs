using System.Security.Claims;

namespace IOweYou.Web.Repositories.User;

public interface IUserRepository : IDbManagement<Models.User>
{
    Task<Models.User?> FindByUsername(string username);
    Task<Models.User?> FindByEmail(string email);
    Task<Models.User?> FindByLogin(string login, string passwordHash);
    Task<Models.User?> GetUserByClaim(ClaimsPrincipal claim);
    Task<List<string>> FindUsernames(string name, bool showMyself, Guid myUserId);


}