using IOweYou.Models;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

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