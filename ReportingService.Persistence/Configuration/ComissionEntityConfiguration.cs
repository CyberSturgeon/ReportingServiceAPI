
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingService.Persistence.Entities;

namespace ReportingService.Persistence.Configuration;

public class ComissionEntityConfiguration : IEntityTypeConfiguration<Comission>
{
    public void Configure(EntityTypeBuilder<Comission> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Income)
            .IsRequired();

        builder.Property(x => x.Transaction)
            .IsRequired();

        builder.Property(x => x.TransactionId)
            .IsRequired();

        builder.HasOne(x => x.Transaction);
    }
}
