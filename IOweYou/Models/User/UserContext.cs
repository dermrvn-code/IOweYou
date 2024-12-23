using Microsoft.EntityFrameworkCore;

namespace IOweYou.Models;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.ID);
    }
}