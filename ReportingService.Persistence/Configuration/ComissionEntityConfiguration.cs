
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

internal static class ComissionEntityConfiguration
{
    internal static void Configure(this ModelBuilder builder)
    {
        builder.Entity<Comission>().Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<Comission>().Property(x => x.Income)
            .IsRequired();

        builder.Entity<Comission>().Property(x => x.Transaction)
            .IsRequired();

        builder.Entity<Comission>().Property(x => x.TransactionID)
            .IsRequired();

        builder.Entity<Comission>().HasOne(x => x.Transaction);

        builder.Entity<Comission>().Property<decimal>(x => x.Income);
    }
}
