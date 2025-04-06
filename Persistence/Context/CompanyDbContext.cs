using Domain.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }



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