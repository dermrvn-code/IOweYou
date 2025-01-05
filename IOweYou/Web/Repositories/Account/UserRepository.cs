using System.Security.Claims;
using IOweYou.Database;
using IOweYou.Models;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.Account;

public class UserRepository : IUserRepository
{
   
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetSingle(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> Add(User entity)
    {
        var user = await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> Update(User entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    
    public async Task<User?> FindByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    
    public async Task<User?> FindByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> FindByLogin(string username, string passwordHash)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
    }

    public async Task<User?> GetUserByClaim(ClaimsPrincipal claim)
    {
        if (!(claim?.Identity?.IsAuthenticated ?? false))
        {
            return null;
        }
        
        var ID = claim.FindFirst("ID")?.Value;

        if (ID == null)
        {
            return null;
        }

        var user = await GetSingle(new Guid(ID));
        return user;
    }

    public async Task<List<string>> FindUsernames(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await _context.Users.Select(u => u.Username).ToListAsync();
        }
        return await _context.Users.Where(u => u.Username.Contains(name))
            .Select(u => u.Username).ToListAsync();
    }
}