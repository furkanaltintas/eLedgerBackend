using Application.Common.Handlers.Companies;
using Application.Common.Interfaces;
using Application.Features.CashRegisters.Constants;
using Application.Features.Companies.Commands;
using Application.Features.Companies.Constants;
using Application.Features.Companies.Rules;
using Domain.Entities.Partners;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Companies.Handlers;

class DeleteCompanyCommandHandler : CompanyCommandHandlerBase, IRequestHandler<DeleteCompanyCommand, IDomainResult<string>>
{
    private readonly CompanyRules _companyRules;

    public DeleteCompanyCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, CompanyRules companyRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _companyRules = companyRules;
    }

    public async Task<IDomainResult<string>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        Company company = await _companyRules.CheckAsync(request.Id, cancellationToken);
        if (company is null) return DomainResult.NotFound<string>(CompaniesMessages.NotFound);

        company.IsDeleted = true;
        return await SuccessAsync(new[] { CashRegistersMessages.Cache }, CashRegistersMessages.Deleted, cancellationToken);
    }
}