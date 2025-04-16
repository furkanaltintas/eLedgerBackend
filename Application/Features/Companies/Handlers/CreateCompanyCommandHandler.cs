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

class CreateCompanyCommandHandler : CompanyCommandHandlerBase, IRequestHandler<CreateCompanyCommand, IDomainResult<string>>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly CompanyRules _companyRules;

    public CreateCompanyCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, ICompanyRepository companyRepository, CompanyRules companyRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _companyRepository = companyRepository;
        _companyRules = companyRules;
    }

    public async Task<IDomainResult<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        IDomainResult<string> taxNumberCheckResult = await _companyRules.CheckTaxNumberExistsAsync(String.Empty, request.TaxNumber, cancellationToken);
        if (!taxNumberCheckResult.IsSuccess) return taxNumberCheckResult;

        Company company = Mapper.Map<Company>(request);
        await _companyRepository.AddAsync(company, cancellationToken);

        return await SuccessAsync(new[] { CompaniesMessages.Cache }, CompaniesMessages.Created, cancellationToken);
    }
}