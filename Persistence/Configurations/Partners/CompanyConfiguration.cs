using Domain.Entities.Partners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Partners
{
    class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(p => p.TaxNumber).HasColumnType("varchar(11)");

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.OwnsOne(p => p.Database, builder =>
            {
                builder.Property(property => property.Server).HasColumnName("Server");
                builder.Property(property => property.DatabaseName).HasColumnName("DatabaseName");
                builder.Property(property => property.UserId).HasColumnName("UserId");
                builder.Property(property => property.Password).HasColumnName("Password");
            });
        }
    }
}
