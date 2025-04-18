using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasQueryFilter(filter => !filter.IsDeleted);
        builder.Property(p => p.Deposit).HasColumnType("decimal(7,2)");
        builder.Property(p => p.Withdrawal).HasColumnType("decimal(7,2)");
    }
}