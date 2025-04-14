using Application.Features.Companies.Constants;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;

namespace Application.Features.Companies.Rules;

public class CompanyRules(ICompanyRepository companyRepository)
{
    public async Task<IDomainResult<string>> CheckTaxNumberExistsAsync(string iban, CancellationToken cancellationToken)
    {
        bool isIBANExists = await companyRepository.AnyAsync(b => b.TaxNumber == iban, cancellationToken);
        return isIBANExists
            ? DomainResult.Failed<string>(CompaniesMessages.AlreadyTaxNumberExists)
            : DomainResult.Success(string.Empty);
    }

    public async Task<Company> CheckAsync(Guid companyId, CancellationToken cancellationToken)
    {
        return await companyRepository.GetByExpressionWithTrackingAsync(b => b.Id == companyId, cancellationToken);
    }
}