using Domain.Entities.Companies;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;
public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.Property(p => p.Amount).HasColumnType("money");
        builder.Property(p => p.Type).HasConversion(type => type.Value, value => InvoiceTypeEnum.FromValue(value));
        builder.HasQueryFilter(filter => !filter.IsDeleted);
        builder.HasQueryFilter(filter => !filter.Customer!.IsDeleted);
    }
}