using System.Security.Claims;
using IOweYou.Database;
using IOweYou.Migrations;
using IOweYou.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Web.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly DatabaseContext _context;

    public UserRepository(ILogger<UserRepository> logger, DatabaseContext context)
    {
        _logger = logger;
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
        if (!(claim?.Identity?.IsAuthenticated ?? false)) return null;

        var ID = claim.FindFirst("ID")?.Value;

        if (ID == null) return null;

        var user = await GetSingle(new Guid(ID));
        return user;
    }

    public async Task<List<string>> FindUsernames(string name, bool showMyself, Guid myUserId)
    {
        if (string.IsNullOrEmpty(name)) return await _context.Users.Select(u => u.Username).ToListAsync();
        return await _context.Users.Where(
                u => u.Username.Contains(name)
                     && (u.ID != myUserId || showMyself)
            )
            .Select(u => u.Username).ToListAsync();
    }

    public async Task<UserToken?> GetToken(string token)
    {
        return await _context.UserToken
            .AsNoTracking()
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.ID.ToString() == token);
    }

    public async Task<bool> AddToken(UserToken token)
    {
        var t = await _context.UserToken.AddAsync(token);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveToken(Guid token)
    {
        var t = await _context.UserToken.FindAsync(token);
        if (t == null) return false;

        _context.UserToken.Remove(t);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> InsertInitUsers()
    {
        var hasher = new PasswordHasher<object>();
        
        var password1 = "isdpassword";
        var hash = hasher.HashPassword(null, password1);
        var user1 = new Models.User("isd_user", "isd@testmail.com", hash)
        {
            ID = new Guid("e201be7e-a500-46d8-b8a3-08f11b7b3f7c"),
            Verified = true
        };
        
        var password2 = "isdpassword2";
        var hash2 = hasher.HashPassword(null, password2);
        var user2 = new Models.User("second_isd_user", "isd2@testmail.com", hash2)
        {
            ID = new Guid("620b15f9-763a-4b4c-ad6b-76b146114f86"),
            Verified = true
        };

        try
        {
            await Add(user1);
            await Add(user2);
        }
        catch
        {
            _logger.LogWarning("Initial users could not be created. They possibly already existed");
            return false;
        }
        return true;
    }
}