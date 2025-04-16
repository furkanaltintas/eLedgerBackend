using Application.Common.Interfaces;
using Application.Features.Companies.Commands;
using Application.Features.Companies.Constants;
using Domain.Entities.Partners;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies.Handlers;

class MigrateAllCompaniesCommandHandler(
    ICompanyRepository companyRepository,
    ICompanyService companyService) : IRequestHandler<MigrateAllCompaniesCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(MigrateAllCompaniesCommand request, CancellationToken cancellationToken)
    {
        List<Company> companies = await companyRepository.GetAll()
                                                         .ToListAsync(cancellationToken);

        companyService.MigrateAllCompanies(companies);
        return DomainResult.Success(CompaniesMessages.DatabasesUpdated);
    }
}