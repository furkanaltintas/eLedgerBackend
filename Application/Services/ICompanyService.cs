using Domain.Entities;

namespace Application.Services;

public interface ICompanyService
{
    void MigrateAllCompanies(List<Company> companies);
}