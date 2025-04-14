using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Services;

class CompanyService : ICompanyService
{
    public void MigrateAllCompanies(List<Company> companies)
    {
        foreach (Company company in companies)
        {
            CompanyDbContext context = new(company);
            context.Database.Migrate();
        }
    }
}
