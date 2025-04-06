using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.UserCompanies;

class UserCompaniesQueryHandler(
    ICompanyUserRepository companyUserRepository) : IRequestHandler<UserCompaniesQuery, IDomainResult<List<Company>>>
{
    public async Task<IDomainResult<List<Company>>> Handle(UserCompaniesQuery request, CancellationToken cancellationToken)
    {
        List<Company> companies = await companyUserRepository
            .GetAll()
            .Where(c => c.AppUserId == Guid.Parse(request.UserId))
            .Include(c => c.Company)
            .Select(c => new Company
            {
                Id = c.CompanyId,
                Name = c.Company!.Name
            })
            .ToListAsync(cancellationToken);

        return DomainResult.Success(companies);
    }
}
