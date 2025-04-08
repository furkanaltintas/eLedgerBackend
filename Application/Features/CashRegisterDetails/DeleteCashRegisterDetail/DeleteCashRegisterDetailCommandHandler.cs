using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using Infrastructure.Services.Cache;
using MediatR;

namespace Application.Features.CashRegisterDetails.DeleteCashRegisterDetail;

class DeleteCashRegisterDetailCommandHandler(
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<DeleteCashRegisterDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteCashRegisterDetailCommand request, CancellationToken cancellationToken)
    {
        CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (cashRegisterDetail is null) return DomainResult<string>.NotFound("Kasa hareketi bulunamadı.");

        CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CashRegisterId, cancellationToken);
        if(cashRegister is null) return DomainResult<string>.NotFound("Kasa bulunamadı.");

        cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
        cashRegister.WithdrawalAmount -= cashRegisterDetail.WithdrawalAmount;

        if(cashRegisterDetail.CashRegisterDetailId is not null)
        {
            CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CashRegisterDetailId, cancellationToken);
            if (oppositeCashRegisterDetail is null) return DomainResult<string>.NotFound("Kasa hareketi bulunamadı.");

            CashRegister? oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);
            if (oppositeCashRegister is null) return DomainResult<string>.NotFound("Kasa bulunamadı.");

            oppositeCashRegister.DepositAmount -= oppositeCashRegisterDetail.DepositAmount;
            oppositeCashRegister.WithdrawalAmount -= oppositeCashRegisterDetail.WithdrawalAmount;

            cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);
        }

        cashRegisterDetailRepository.Delete(cashRegisterDetail);

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return DomainResult.Success("Kasa hareketi başarıyla silindi.");
    }
}
