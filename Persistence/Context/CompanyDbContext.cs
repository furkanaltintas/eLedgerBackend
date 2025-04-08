using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Persistence.Context;

public class CompanyDbContext : DbContext, IUnitOfWorkCompany
{
    private string connectionString = string.Empty;

    public CompanyDbContext(Company company)
    {
        CreateConnectionStringWithCompany(company);
    }

    public CompanyDbContext(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        CreateConnectionString(httpContextAccessor, context);
    }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region CashRegister
        builder.Entity<CashRegister>().Property(p => p.DepositAmount).HasColumnType("money");
        builder.Entity<CashRegister>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Entity<CashRegister>().Property(p => p.CurrencyType).HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
        builder.Entity<CashRegister>().HasQueryFilter(filter => !filter.IsDeleted);
        builder.Entity<CashRegister>().HasMany(p => p.Details).WithOne().HasForeignKey(p => p.CashRegisterId);
        #endregion

        #region CashRegisterDetail
        builder.Entity<CashRegisterDetail>().Property(p => p.DepositAmount).HasColumnType("money");
        builder.Entity<CashRegisterDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Entity<CashRegister>().HasQueryFilter(filter => !filter.IsDeleted);
        #endregion
    }

    public DbSet<CashRegister> CashRegisters { get; set; }
    public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }








    private void CreateConnectionString(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        if (httpContextAccessor.HttpContext is null) return;

        String? companyId = httpContextAccessor.HttpContext.User.FindFirstValue("CompanyId");
        if (string.IsNullOrEmpty(companyId)) return;

        Company? company = context.Companies.Find(Guid.Parse(companyId));
        if (company is null) return;


        CreateConnectionStringWithCompany(company);
    }

    private void CreateConnectionStringWithCompany(Company company)
    {
        Database db = company.Database;
        String authPart = string.IsNullOrEmpty(db.UserId)
            ? "Integrated Security=True;"
            : $"Integrated Security=False;User Id={db.UserId};Password={db.Password};";

        connectionString =
            $"Server={db.Server};" +
            $"Database={db.DatabaseName};" +
            authPart +
            "Connection Timeout=30;" +
            "Encrypt=True;" +
            "Trust Server Certificate=True;" +
            "Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False;";
    }
}