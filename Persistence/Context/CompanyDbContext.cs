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
    #region Context
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
    #endregion


    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region Bank
        builder.Entity<Bank>().Property(p => p.DepositAmount).HasColumnType("money");
        builder.Entity<Bank>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Entity<Bank>().Property(p => p.CurrencyType).HasConversion(type => type.Value, value => CurrencyTypeEnum.FromValue(value));
        builder.Entity<Bank>().HasQueryFilter(filter => !filter.IsDeleted);
        builder.Entity<Bank>().HasMany(p => p.Details).WithOne().HasForeignKey(p => p.BankId);
        #endregion

        #region BankDetail
        builder.Entity<BankDetail>().Property(p => p.DepositAmount).HasColumnType("money");
        builder.Entity<BankDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        #endregion

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
        #endregion

        #region Customer
        builder.Entity<Customer>().Property(p => p.DepositAmount).HasColumnType("money");
        builder.Entity<Customer>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Entity<Customer>().Property(p => p.Type).HasConversion(type => type.Value, value => CustomerTypeEnum.FromValue(value));
        builder.Entity<Customer>().HasQueryFilter(filter => !filter.IsDeleted);
        #endregion

        #region CustomerDetail
        builder.Entity<CustomerDetail>().Property(p => p.DepositAmount).HasColumnType("money");
        builder.Entity<CustomerDetail>().Property(p => p.WithdrawalAmount).HasColumnType("money");
        builder.Entity<CustomerDetail>().Property(p => p.Type).HasConversion(type => type.Value, value => CustomerDetailTypeEnum.FromValue(value));
        #endregion

        #region Invoice
        builder.Entity<Invoice>().Property(p => p.Amount).HasColumnType("money");
        builder.Entity<Invoice>().Property(p => p.Type).HasConversion(type => type.Value, value => InvoiceTypeEnum.FromValue(value));
        builder.Entity<Invoice>().HasQueryFilter(filter => !filter.IsDeleted);
        builder.Entity<Invoice>().HasQueryFilter(filter => !filter.Customer!.IsDeleted);
        #endregion

        #region InvoiceDetail
        builder.Entity<InvoiceDetail>().Property(p => p.Quantity).HasColumnType("decimal(7,2)");
        builder.Entity<InvoiceDetail>().Property(p => p.Price).HasColumnType("money");
        builder.Entity<InvoiceDetail>().HasQueryFilter(filter => !filter.Product!.IsDeleted);
        #endregion

        #region ProductDetail
        builder.Entity<ProductDetail>().Property(p => p.Deposit).HasColumnType("decimal(7,2)");
        builder.Entity<ProductDetail>().Property(p => p.Withdrawal).HasColumnType("decimal(7,2)");
        builder.Entity<ProductDetail>().Property(p => p.Price).HasColumnType("money");
        #endregion

        #region Product
        builder.Entity<Product>().HasQueryFilter(filter => !filter.IsDeleted);
        builder.Entity<Product>().Property(p => p.Deposit).HasColumnType("decimal(7,2)");
        builder.Entity<Product>().Property(p => p.Withdrawal).HasColumnType("decimal(7,2)");
        #endregion

    }

    public DbSet<Bank> Banks { get; set; }
    public DbSet<BankDetail> BankDetails { get; set; }
    public DbSet<CashRegister> CashRegisters { get; set; }
    public DbSet<CashRegisterDetail> CashRegisterDetails { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerDetail> CustomerDetails { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductDetail> ProductDetails { get; set; }


    #region Connection
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
    #endregion
}