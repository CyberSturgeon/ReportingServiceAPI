using Microsoft.EntityFrameworkCore;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

internal static class AccountEntityConfiguration
{
    internal static void ConfigureAccount(this ModelBuilder builder)
	{
        builder.Entity<Account>().Property(x => x.Id)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder.Entity<Account>().Property<DateTime>(x => x.DateCreated);

        builder.Entity<Account>().Property(x => x.Status)
        .IsRequired();

        builder.Entity<Account>().Property(x => x.Currency)
        .IsRequired();

        builder.Entity<Account>().HasOne(x => x.Customer)
        .WithMany(y => y.Accounts)
        .HasForeignKey(x => x.CustomerId);

        builder.Entity<Account>().HasMany(x => x.Transactions);

    }
}
