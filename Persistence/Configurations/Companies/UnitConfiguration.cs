using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasQueryFilter(filter => !filter.IsDeleted);
        builder.Property(u => u.Name).HasMaxLength(50);
        builder.Property(u => u.Symbol).HasMaxLength(25);
    }
}