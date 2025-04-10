using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.DeleteBankDetail;

class DeleteBankDetailCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
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

        // Bankadan kasaya aktarım yapıldıysa
        if (bankDetail.CashRegisterDetailId is not null)
        {
            CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == bankDetail.CashRegisterDetailId, cancellationToken);
            if (oppositeCashRegisterDetail is null) return DomainResult<string>.NotFound("Kasa hareketi bulunamadı.");

            CashRegister? oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);
            if (oppositeCashRegister is null) return DomainResult<string>.NotFound("Kasa bulunamadı.");

            oppositeCashRegister.DepositAmount -= oppositeCashRegisterDetail.DepositAmount;
            oppositeCashRegister.WithdrawalAmount -= oppositeCashRegisterDetail.WithdrawalAmount;

            cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);
        }

        if (bankDetail.CustomerDetailId is not null)
        {
            CustomerDetail? oppositeCustomerDetail = await customerDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == bankDetail.CustomerDetailId, cancellationToken);
            if (oppositeCustomerDetail is null) return DomainResult<string>.NotFound("Cari hareketi bulunamadı.");

            Customer? oppositeCustomer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeCustomerDetail.CustomerId, cancellationToken);
            if (oppositeCustomer is null) return DomainResult<string>.NotFound("Cari bulunamadı.");

            oppositeCustomer.DepositAmount -= oppositeCustomerDetail.DepositAmount;
            oppositeCustomer.WithdrawalAmount -= oppositeCustomerDetail.WithdrawalAmount;

            customerDetailRepository.Delete(oppositeCustomerDetail);
        }

        bankDetailRepository.Delete(bankDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveRangeCompanyFromContext(new[] { "banks", "cashRegisters", "customers" });

        return DomainResult.Success("Banka hareketi başarıyla silindi.");
    }
}