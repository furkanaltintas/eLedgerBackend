using Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Companies;


public class BankDetailConfiguration : IEntityTypeConfiguration<BankDetail>
{
    public void Configure(EntityTypeBuilder<BankDetail> builder)
    {
        builder.Property(p => p.DepositAmount).HasColumnType("money");
        builder.Property(p => p.WithdrawalAmount).HasColumnType("money");
    }
}