using Application.Common.Handlers.Partners;
using Application.Common.Interfaces;
using Application.Features.Companies.Constants;
using Application.Features.Companies.Queries;
using Domain.Entities.Partners;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Handlers;

class GetAllCompaniesQueryHandler : ApplicationCacheableQueryHandlerBase, IRequestHandler<GetAllCompaniesQuery, IDomainResult<List<Company>>>
{
    private readonly ICompanyRepository _companyRepository;
    public GetAllCompaniesQueryHandler(ICacheService cacheService, ICompanyRepository companyRepository) : base(cacheService)
    {
        _companyRepository = companyRepository;
    }

    public async Task<IDomainResult<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await Success(CompaniesMessages.Cache, async () =>
        {
            return await _companyRepository.GetAll().OrderBy(c => c.Name).ToListAsync(cancellationToken);
        });
    }
}