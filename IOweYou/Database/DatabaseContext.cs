using IOweYou.Models;
using IOweYou.Models.Transactions;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Balance> Balances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(a => a.User)
            .HasForeignKey(t => t.UserId);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.ExternalTransactions)
            .WithOne(a => a.Partner)
            .HasForeignKey(t => t.PartnerId);

        modelBuilder.Entity<User>()
            .Property(u => u.DateCreated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Currency);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Date)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        modelBuilder.Entity<Balance>()
            .HasOne(b => b.FromUser)
            .WithMany(u => u.FromBalances)
            .HasForeignKey(b => b.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Balance>()
            .HasOne(b => b.ToUser)
            .WithMany(u => u.ToBalances)
            .HasForeignKey(b => b.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Balance>()
            .HasOne(b => b.Currency)
            .WithMany(c => c.Balances)
            .HasForeignKey(b => b.CurrencyId);

        modelBuilder.Entity<Balance>()
            .Property(b => b.LastUpdated)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
    }
}