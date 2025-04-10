using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.DeleteCashRegisterDetail;

class DeleteCashRegisterDetailCommandHandler(
    ICustomerRepository customerRepository,
    ICustomerDetailRepository customerDetailRepository,
    IBankRepository bankRepository,
    IBankDetailRepository bankDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<DeleteCashRegisterDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteCashRegisterDetailCommand request, CancellationToken cancellationToken)
    {
        CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (cashRegisterDetail is null) return DomainResult<string>.NotFound("Kasa hareketi bulunamadı.");

        CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CashRegisterId, cancellationToken);
        if (cashRegister is null) return DomainResult<string>.NotFound("Kasa bulunamadı.");

        cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
        cashRegister.WithdrawalAmount -= cashRegisterDetail.WithdrawalAmount;

        if (cashRegisterDetail.CashRegisterDetailId is not null)
        {
            CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CashRegisterDetailId, cancellationToken);
            if (oppositeCashRegisterDetail is null) return DomainResult<string>.NotFound("Kasa hareketi bulunamadı.");

            CashRegister? oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);
            if (oppositeCashRegister is null) return DomainResult<string>.NotFound("Kasa bulunamadı.");

            oppositeCashRegister.DepositAmount -= oppositeCashRegisterDetail.DepositAmount;
            oppositeCashRegister.WithdrawalAmount -= oppositeCashRegisterDetail.WithdrawalAmount;

            cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);
        }

        if (cashRegisterDetail.BankDetailId is not null)
        {
            BankDetail? oppositeBankDetail = await bankDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.BankDetailId, cancellationToken);
            if (oppositeBankDetail is null) return DomainResult<string>.NotFound("Banka hareketi bulunamadı.");

            Bank? oppositeBank = await bankRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeBankDetail.BankId, cancellationToken);
            if (oppositeBank is null) return DomainResult<string>.NotFound("Banka bulunamadı.");

            oppositeBank.DepositAmount -= oppositeBankDetail.DepositAmount;
            oppositeBank.WithdrawalAmount -= oppositeBankDetail.WithdrawalAmount;

            bankDetailRepository.Delete(oppositeBankDetail);
        }

        if (cashRegisterDetail.CustomerDetailId is not null)
        {
            CustomerDetail? oppositeCustomerDetail = await customerDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CustomerDetailId, cancellationToken);
            if (oppositeCustomerDetail is null) return DomainResult<string>.NotFound("Cari hareketi bulunamadı.");

            Customer? oppositeCustomer = await customerRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeCustomerDetail.CustomerId, cancellationToken);
            if (oppositeCustomer is null) return DomainResult<string>.NotFound("Cari bulunamadı.");

            oppositeCustomer.DepositAmount -= oppositeCustomerDetail.DepositAmount;
            oppositeCustomer.WithdrawalAmount -= oppositeCustomerDetail.WithdrawalAmount;

            customerDetailRepository.Delete(oppositeCustomerDetail);
        }

        cashRegisterDetailRepository.Delete(cashRegisterDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        companyContextHelper.RemoveRangeCompanyFromContext(new[] { "banks", "cashRegisters" });

        return DomainResult.Success("Kasa hareketi başarıyla silindi.");
    }
}