using Domain.Entities.Companies;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(p => p.DepositAmount).HasColumnType("money");
        builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Property(p => p.Type).HasConversion(type => type.Value, value => CustomerTypeEnum.FromValue(value));
        builder.HasQueryFilter(filter => !filter.IsDeleted);
    }
}