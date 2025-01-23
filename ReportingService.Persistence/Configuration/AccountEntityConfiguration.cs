using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

internal static class AccountEntityConfiguration
{
    internal static void ConfigureAccount(this ModelBuilder builder)
	{
        builder.Entity<Account>().Property(x => x.Id)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder.Entity<Account>().Property(x => x.CustomerId)
       .IsRequired()

        builder.Entity<Account>().Property<DateTime>(x => x.DateCreated);

        uilder.Entity<Account>().Property(x => x.Status)
        .IsRequired();

        builder.Entity<Account>().Property(x => x.Currensy)
        .IsRequired();

        builder.Entity<Account>().Property(x => x.Customer)
        .IsRequired();

        builder.Entity<Account>().HasMany(x => x.Transactions)
        
    }
}
