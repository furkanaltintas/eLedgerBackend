using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.Companies.Commands;
using Application.Features.Companies.Constants;
using Application.Features.Companies.Rules;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Companies.Handlers;

class UpdateCompanyCommandHandler : BaseCommandHandler, IRequestHandler<UpdateCompanyCommand, IDomainResult<string>>
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

        if (company.TaxNumber != request.TaxNumber)
        {
            IDomainResult<string> taxNumberCheckResult = await _companyRules.CheckTaxNumberExistsAsync(request.TaxNumber, cancellationToken);
            if (!taxNumberCheckResult.IsSuccess) return taxNumberCheckResult;
        }

        Mapper.Map(request, company);
        return await Success(new[] { CompaniesMessages.Cache }, CompaniesMessages.Updated, cancellationToken);
    }
}