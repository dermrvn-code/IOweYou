using System.Security.Claims;
using IOweYou.Models;
using IOweYou.Web.Repositories;

namespace IOweYou.Web.Services;

public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User?> GetSingle(Guid id)
    {
        return await _userRepository.GetSingle(id);
    }

    public async Task<bool> Add(User entity)
    {
        return await _userRepository.Add(entity);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.Delete(id);
    }

    public async Task<bool> Update(User entity)
    {
        return await _userRepository.Update(entity);
    }

    public async Task<User?> FindByUsername(string username)
    {
        return await _userRepository.FindByUsername(username);
    }

    public async Task<User?> FindByEmail(string email)
    {
        return await _userRepository.FindByEmail(email);
    }

    public async Task<User?> FindByLogin(string login, string passwordHash)
    {
        return await _userRepository.FindByLogin(login, passwordHash);
    }

    public async Task<User?> GetUserByClaim(ClaimsPrincipal claim)
    {
        return await _userRepository.GetUserByClaim(claim);
    }

    public async Task<List<string>> FindUsernames(string name)
    {
        return await _userRepository.FindUsernames(name);
    }
    
    
}