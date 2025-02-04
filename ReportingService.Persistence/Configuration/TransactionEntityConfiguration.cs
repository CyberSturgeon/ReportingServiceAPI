using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

internal static class TransactionEntityConfiguration
{
    internal static void ConfigureTransaction(this ModelBuilder builder)
    {
        builder.Entity<Transaction>().Property(x => x.Id)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder.Entity<Transaction>().Property(x => x.AccountId)
       .IsRequired();

        builder.Entity<Transaction>().Property<decimal>(x => x.Amount);

        builder.Entity<Transaction>().Property<DateTime>(x => x.Date);

        builder.Entity<Transaction>().Property<decimal>(x => x.AmountRUB);

        builder.Entity<Transaction>().Property(x => x.TransactionType)
        .IsRequired();

        builder.Entity<Transaction>().Property(x => x.Currency)
        .IsRequired();

        builder.Entity<Transaction>().Property(x => x.CustomerId)
        .IsRequired(); 
    }
}
