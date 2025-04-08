using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.DeleteBankDetail;

class DeleteBankDetailCommandHandler(
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<DeleteBankDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteBankDetailCommand request, CancellationToken cancellationToken)
    {
        BankDetail? bankDetail = await bankDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (bankDetail is null) return DomainResult<string>.NotFound("Banka hareketi bulunamadı.");

        Bank? bank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == bankDetail.BankId, cancellationToken);
        if (bank is null) return DomainResult<string>.NotFound("Banka bulunamadı.");

        bank.DepositAmount -= bankDetail.DepositAmount;
        bank.WithdrawalAmount -= bankDetail.WithdrawalAmount;

        if (bankDetail.BankDetailId is not null)
        {
            BankDetail? oppositeBankDetail = await bankDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == bankDetail.BankDetailId, cancellationToken);
            if (oppositeBankDetail is null) return DomainResult<string>.NotFound("Banka hareketi bulunamadı.");

            Bank? oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeBankDetail.BankId, cancellationToken);
            if (oppositeBank is null) return DomainResult<string>.NotFound("Banka bulunamadı.");

            oppositeBank.DepositAmount -= oppositeBankDetail.DepositAmount;
            oppositeBank.WithdrawalAmount -= oppositeBankDetail.WithdrawalAmount;

            bankDetailRepository.Delete(oppositeBankDetail);
        }

        bankDetailRepository.Delete(bankDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("banks");

        return DomainResult.Success("Banka hareketi başarıyla silindi.");
    }
}