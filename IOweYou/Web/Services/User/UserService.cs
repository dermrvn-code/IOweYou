using System.Security.Claims;
using IOweYou.Models;
using IOweYou.Web.Repositories.User;

namespace IOweYou.Web.Services.User;

public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<Models.User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<Models.User?> GetSingle(Guid id)
    {
        return await _userRepository.GetSingle(id);
    }

    public async Task<bool> Add(Models.User entity)
    {
        return await _userRepository.Add(entity);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.Delete(id);
    }

    public async Task<bool> Update(Models.User entity)
    {
        return await _userRepository.Update(entity);
    }

    public async Task<Models.User?> FindByUsername(string username)
    {
        return await _userRepository.FindByUsername(username);
    }

    public async Task<Models.User?> FindByEmail(string email)
    {
        return await _userRepository.FindByEmail(email);
    }

    public async Task<Models.User?> FindByLogin(string login, string passwordHash)
    {
        return await _userRepository.FindByLogin(login, passwordHash);
    }

    public async Task<Models.User?> GetUserByClaim(ClaimsPrincipal claim)
    {
        return await _userRepository.GetUserByClaim(claim);
    }

    public async Task<List<string>> FindUsernames(string name, bool showMyself, Guid myUserId)
    {
        return await _userRepository.FindUsernames(name, showMyself, myUserId);
    }

    public async Task<UserToken?> GetToken(string token)
    {
        return await _userRepository.GetToken(token);
    }

    public async Task<bool> AddToken(UserToken token)
    {
        return await _userRepository.AddToken(token);
    }

    public async Task<bool> RemoveToken(Guid token)
    {
        return await _userRepository.RemoveToken(token);
    }
}