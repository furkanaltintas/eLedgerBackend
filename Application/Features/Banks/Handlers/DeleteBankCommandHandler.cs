using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.Banks.Commands;
using Application.Features.Banks.Constants;
using Application.Features.Banks.Rules;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Banks.Handlers;

class DeleteBankCommandHandler : BaseCommandHandler, IRequestHandler<DeleteBankCommand, IDomainResult<string>>
{
    private readonly BankRules _bankRules;
    public DeleteBankCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, BankRules bankRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _bankRules = bankRules;
    }

    public async Task<IDomainResult<string>> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
    {
        Bank? bank = await _bankRules.CheckAsync(request.Id, cancellationToken);
        if (bank is null) return DomainResult.Failed<string>(BankDetailsMessages.NotFound);

        bank.IsDeleted = true;
        return await Success(BankDetailsMessages.Cache, BankDetailsMessages.Deleted, cancellationToken);
    }
}