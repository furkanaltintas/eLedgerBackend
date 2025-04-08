using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.GetAllCompanies;

class GetAllCompaniesQueryHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService) : IRequestHandler<GetAllCompaniesQuery, IDomainResult<List<Company>>>
{
    public async Task<IDomainResult<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        List<Company> companies;

        companies = cacheService.Get<List<Company>>("companies");
        if (companies is null)
        {
            companies = await companyRepository
                .GetAll()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            cacheService.Set("companies", companies);

            return DomainResult.Success(companies);
        }

        return DomainResult.Success(companies);
    }
}
