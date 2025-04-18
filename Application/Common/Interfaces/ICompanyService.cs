using Domain.Entities.Partners;

namespace Application.Common.Interfaces;

public interface ICompanyService
{
    void MigrateAllCompanies(List<Company> companies);
}