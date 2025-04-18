using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;
public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder.Property(p => p.Quantity).HasColumnType("decimal(7,2)");
        builder.Property(p => p.Price).HasColumnType("money");
        builder.HasQueryFilter(filter => !filter.Product!.IsDeleted);
    }
}