using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.CreateBankDetail;

class CreateBankDetailCommandHandler(
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<CreateBankDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
    {
        Bank bank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.BankId, cancellationToken);
        bank.DepositAmount += (request.Type == 0 ? request.Amount : 0);
        bank.WithdrawalAmount += (request.Type == 1 ? request.Amount : 0);

        BankDetail bankDetail = new()
        {
            Date = request.Date,
            DepositAmount = request.Type == 0 ? request.Amount : 0,
            WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
            Description = request.Description,
            BankId = request.BankId
        };

        await bankDetailRepository.AddAsync(bankDetail, cancellationToken);

        if (request.OppositeBankId is not null)
        {
            Bank oppositeCashRegister = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeBankId, cancellationToken);

            oppositeCashRegister.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeCashRegister.WithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            BankDetail oppositeBankDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                BankDetailId = bankDetail.Id,
                Description = request.Description,
                BankId = (Guid)request.OppositeBankId,
            };

            bankDetail.BankDetailId = oppositeBankDetail.Id;

            await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);
        }

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("banks");

        return DomainResult.Success("Bank detail created successfully.");
    }
}
