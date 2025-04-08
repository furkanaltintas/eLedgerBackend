using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.UpdateBankDetail;

class UpdateBankDetailCommandHandler(
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<UpdateBankDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateBankDetailCommand request, CancellationToken cancellationToken)
    {
        BankDetail? bankDetail = await bankDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (bankDetail is null) return DomainResult<string>.NotFound("Banka hareketi bulunamadı.");

        Bank? bank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == bankDetail.BankId, cancellationToken);
        if (bank is null) return DomainResult<string>.NotFound("Banka bulunamadı.");

        bank.DepositAmount -= bankDetail.DepositAmount;
        bank.WithdrawalAmount -= bankDetail.WithdrawalAmount;

        bank.DepositAmount += request.Type == 0 ? request.Amount : 0;
        bank.WithdrawalAmount += request.Type == 1 ? request.Amount : 0;

        bankDetail.DepositAmount = request.Type == 0 ? request.Amount : 0;
        bankDetail.WithdrawalAmount = request.Type == 1 ? request.Amount : 0;

        bankDetail.Description = request.Description;
        bankDetail.Date = request.Date;

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveCompanyFromContext("banks");

        return DomainResult.Success("Banka hareketi başarıyla güncellendi");
    }
}