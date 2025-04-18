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

class UpdateBankCommandHandler : BaseCommandHandler, IRequestHandler<UpdateBankCommand, IDomainResult<string>>
{
    private readonly BankRules _bankRules;
    public UpdateBankCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, BankRules bankRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _bankRules = bankRules;
    }

    public async Task<IDomainResult<string>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        Bank? bank = await _bankRules.CheckAsync(request.Id, cancellationToken);
        if(bank is null) return DomainResult.Failed<string>(BankDetailsMessages.NotFound);

        if (bank.IBAN != request.IBAN)
        {
            IDomainResult<string> isIBANExistsResult = await _bankRules.CheckIBANExistsAsync(request.IBAN, cancellationToken);
            if (!isIBANExistsResult.IsSuccess) return isIBANExistsResult;
        }

        Mapper.Map(request, bank);
        return await Success(BankDetailsMessages.Cache, BankDetailsMessages.Updated, cancellationToken);
    }
}