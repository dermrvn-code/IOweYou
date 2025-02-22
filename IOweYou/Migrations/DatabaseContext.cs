﻿using IOweYou.Models;
using IOweYou.Models.Transactions;
using Microsoft.EntityFrameworkCore;

namespace IOweYou.Migrations;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Balance> Balances { get; set; }
    public DbSet<UserToken> UserToken { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Balances)
            .WithOne(b => b.FromUser)
            .HasForeignKey(b => b.FromUserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserID);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Currency);
        
        modelBuilder.Entity<Transaction>()
            .Navigation(t => t.Currency)
            .AutoInclude();

        modelBuilder.Entity<Balance>()
            .HasOne(b => b.Currency);
    }
}