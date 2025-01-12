using System.Security.Claims;
using IOweYou.Database;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.User;

public class UserRepository : IUserRepository
{
   
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Models.User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Models.User?> GetSingle(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> Add(Models.User entity)
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
    
    public async Task<bool> Update(Models.User entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    
    public async Task<Models.User?> FindByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    
    public async Task<Models.User?> FindByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<Models.User?> FindByLogin(string username, string passwordHash)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
    }

    public async Task<Models.User?> GetUserByClaim(ClaimsPrincipal claim)
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

    public async Task<List<string>> FindUsernames(string name, bool showMyself, Guid myUserId)
    {
        if (string.IsNullOrEmpty(name))
        {
            return await _context.Users.Select(u => u.Username).ToListAsync();
        }
        return await _context.Users.Where(
                u => u.Username.Contains(name)
                && (u.ID != myUserId || showMyself)
            )
            .Select(u => u.Username).ToListAsync();
    }
}