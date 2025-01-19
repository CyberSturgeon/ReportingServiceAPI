using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasMany(x => x.Accounts).WithOne(x => x.Customer);

        builder.HasMany(x => x.Transactions);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Login)
        .IsUnique();

        builder.Property(x => x.FirstName)
        .IsRequired()
        .HasMaxLength(30);

        builder.Property(x => x.LastName)
        .IsRequired()
        .HasMaxLength(30);

        builder.Property(x => x.Login)
        .IsRequired()
        .HasMaxLength(10);

        builder.Property(x => x.Password)
        .IsRequired()
        .HasMaxLength(150);

        builder.Property(x => x.Role)
        .IsRequired();

        builder.Property(x => x.IsDeactivated)
        .IsRequired();
    }
}
