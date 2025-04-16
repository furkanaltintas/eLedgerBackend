using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.Companies.Commands;
using Application.Features.Companies.Constants;
using Application.Features.Companies.Rules;
using Domain.Entities.Partners;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Companies.Handlers;

class UpdateCompanyCommandHandler : CompanyCommandHandlerBase, IRequestHandler<UpdateCompanyCommand, IDomainResult<string>>
{
    private readonly CompanyRules _companyRules;

    public UpdateCompanyCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, CompanyRules companyRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _companyRules = companyRules;
    }

    public async Task<IDomainResult<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await _companyRules.CheckAsync(request.Id, cancellationToken);
        if (company is null) return DomainResult.NotFound<string>(CompaniesMessages.NotFound);

        IDomainResult<string> taxNumberCheckResult = await _companyRules.CheckTaxNumberExistsAsync(company.TaxNumber, request.TaxNumber, cancellationToken);
        if (!taxNumberCheckResult.IsSuccess) return taxNumberCheckResult;

        Mapper.Map(request, company);
        return await SuccessAsync(new[] { CompaniesMessages.Cache }, CompaniesMessages.Updated, cancellationToken);
    }
}