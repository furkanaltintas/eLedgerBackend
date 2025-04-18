using Domain.Entities.Companies;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;

public class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegister>
{
    public void Configure(EntityTypeBuilder<CashRegister> builder)
    {
        builder.Property(p => p.DepositAmount).HasColumnType("money");
        builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Property(p => p.CurrencyType).HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
        builder.HasQueryFilter(filter => !filter.IsDeleted);
        builder.HasMany(p => p.Details).WithOne().HasForeignKey(p => p.CashRegisterId);
    }
}