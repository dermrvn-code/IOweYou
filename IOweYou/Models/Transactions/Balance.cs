﻿namespace IOweYou.Models.Transactions;

public class Balance : Entity
{
    public Guid FromUserId { get; set; }
    public User FromUser { get; set; }
    public Guid ToUserId { get; set; }
    public User ToUser { get; set; }
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; } 
    public DateTime LastUpdated { get; set; }
    
    public Balance() { }
    
    public Balance(User fromUser, User toUser, Currency currency, decimal amount)
    {
        FromUser = fromUser;
        ToUser = toUser;
        Currency = currency;
        Amount = amount;
        LastUpdated = DateTime.Now;
    }
}