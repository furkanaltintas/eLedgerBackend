using Application.Features.Companies.Constants;
using Domain.Entities.Partners;
using Domain.Interfaces;
using DomainResults.Common;

namespace Application.Features.Companies.Rules;

public class CompanyRules(ICompanyRepository companyRepository)
{
    /// <summary>
    /// Create işleminde 'currentTaxNumber' yapısına String.Empty değerini atayın.
    /// </summary>
    /// <param name="currentTaxNumber"></param>
    /// <param name="newTaxNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IDomainResult<string>> CheckTaxNumberExistsAsync(string currentTaxNumber, string newTaxNumber, CancellationToken cancellationToken)
    {
        if (currentTaxNumber != newTaxNumber)
        {
            bool isTaxNumberExists = await companyRepository.AnyAsync(b => b.TaxNumber == newTaxNumber, cancellationToken);
            if (isTaxNumberExists) DomainResult.Failed<string>(CompaniesMessages.AlreadyTaxNumberExists);
        }

        return DomainResult.Success(string.Empty);
    }

    public async Task<Company> CheckAsync(Guid companyId, CancellationToken cancellationToken)
    {
        return await companyRepository.GetByExpressionWithTrackingAsync(b => b.Id == companyId, cancellationToken);
    }
}