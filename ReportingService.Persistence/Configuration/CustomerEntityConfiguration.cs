using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

internal static class CustomerEntityConfiguration
{
    internal static void ConfigureCustomer(this ModelBuilder builder)
    {
        builder.Entity<Customer>().HasMany(x => x.Accounts)
                .WithOne(y => y.Customer)
                .HasForeignKey(x => x.CustomerID);

        builder.Entity<Customer>().HasMany(x => x.Transactions);

        builder.Entity<Customer>().Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Customer>().HasIndex(x => x.Login)
        .IsUnique();

        builder.Entity<Customer>().Property(x => x.FirstName)
        .IsRequired()
        .HasMaxLength(30);

        builder.Entity<Customer>().Property(x => x.LastName)
        .IsRequired()
        .HasMaxLength(30);

        builder.Entity<Customer>().Property(x => x.Login)
        .IsRequired()
        .HasMaxLength(10);

        builder.Entity<Customer>().Property(x => x.Password)
        .IsRequired()
        .HasMaxLength(150);

        builder.Entity<Customer>().Property(x => x.Role)
        .IsRequired();

        builder.Entity<Customer>().Property(x => x.IsDeactivated)
        .IsRequired();

        builder.Entity<Customer>().Property<DateTime>(x => x.BirthDate);
    }
}
