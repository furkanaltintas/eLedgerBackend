using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICompanyService
{
    void MigrateAllCompanies(List<Company> companies);
}