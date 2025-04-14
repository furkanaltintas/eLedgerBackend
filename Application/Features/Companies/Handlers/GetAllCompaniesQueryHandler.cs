using Application.Common.Interfaces;
using Application.Features.Companies.Constants;
using Application.Features.Companies.Queries;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Handlers;

class GetAllCompaniesQueryHandler(
    ICompanyRepository companyRepository,
    ICacheService cacheService) : IRequestHandler<GetAllCompaniesQuery, IDomainResult<List<Company>>>
{
    public async Task<IDomainResult<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        List<Company> companies = cacheService.Get<List<Company>>(CompaniesMessages.Cache);

        if (companies is null)
        {
            companies = await companyRepository.GetAll().OrderBy(c => c.Name).ToListAsync(cancellationToken);
            cacheService.Set(CompaniesMessages.Cache, companies);
        }

        return DomainResult.Success(companies);
    }
}
