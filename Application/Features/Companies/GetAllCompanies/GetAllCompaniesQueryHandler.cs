using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.GetAllCompanies;

class GetAllCompaniesQueryHandler(ICompanyRepository companyRepository) : IRequestHandler<GetAllCompaniesQuery, IDomainResult<List<Company>>>
{
    public async Task<IDomainResult<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        List<Company> companies = await companyRepository
            .GetAll()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
        return DomainResult.Success(companies);
    }
}
