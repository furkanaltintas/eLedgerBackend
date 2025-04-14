using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.BankDetails.Commands;
using Application.Features.BankDetails.Constants;
using Application.Features.BankDetails.Rules;
using Application.Features.Banks.Constants;
using Application.Features.Banks.Rules;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.BankDetails.Handlers;

class UpdateBankDetailCommandHandler : BaseCommandHandler, IRequestHandler<UpdateBankDetailCommand, IDomainResult<string>>
{
    private readonly IBankDetailRepository _bankDetailRepository;
    private readonly BankDetailRules _bankDetailRules;
    private readonly BankRules _bankRules;

    public UpdateBankDetailCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, IBankDetailRepository bankDetailRepository, BankDetailRules bankDetailRules, BankRules bankRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _bankDetailRepository = bankDetailRepository;
        _bankDetailRules = bankDetailRules;
        _bankRules = bankRules;
    }

    public async Task<IDomainResult<string>> Handle(UpdateBankDetailCommand request, CancellationToken cancellationToken)
    {
        BankDetail? bankDetail = await _bankDetailRules.CheckAsync(request.Id, cancellationToken);
        if(bankDetail is null) return DomainResult<string>.NotFound(BankDetailsMessages.NotFound);

        Bank? bank = await _bankRules.CheckAsync(bankDetail.BankId, cancellationToken);
        if (bank is null) return DomainResult<string>.NotFound(BanksMessages.NotFound);

        bank.DepositAmount -= bankDetail.DepositAmount;
        bank.WithdrawalAmount -= bankDetail.WithdrawalAmount;

        bank.DepositAmount += request.Type == 0 ? request.Amount : 0;
        bank.WithdrawalAmount += request.Type == 1 ? request.Amount : 0;

        bankDetail.DepositAmount = request.Type == 0 ? request.Amount : 0;
        bankDetail.WithdrawalAmount = request.Type == 1 ? request.Amount : 0;

        bankDetail.Description = request.Description;
        bankDetail.Date = request.Date;

        return Success(new[] { BanksMessages.Cache, CashRegistersMessages.Cache }, BankDetailsMessages.Updated, cancellationToken);
    }
}