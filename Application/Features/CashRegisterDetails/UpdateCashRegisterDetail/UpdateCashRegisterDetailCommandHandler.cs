using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using Infrastructure.Services.Cache;
using MediatR;

namespace Application.Features.CashRegisterDetails.UpdateCashRegisterDetail;

class UpdateCashRegisterDetailCommandHandler(
    ICashRegisterRepository cashRegisterRepository,
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<UpdateCashRegisterDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateCashRegisterDetailCommand request, CancellationToken cancellationToken)
    {
        CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (cashRegisterDetail is null) return DomainResult<string>.NotFound("Kasa hareketi bulunamadı.");

        CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterDetail.CashRegisterId, cancellationToken);
        if (cashRegister is null) return DomainResult<string>.NotFound("Kasa bulunamadı.");

        cashRegister.DepositAmount -= cashRegisterDetail.DepositAmount;
        cashRegister.WithdrawalAmount -= cashRegisterDetail.WithdrawalAmount;

        cashRegister.DepositAmount += request.Type == 0 ? request.Amount : 0;
        cashRegister.WithdrawalAmount += request.Type == 1 ? request.Amount : 0;

        cashRegisterDetail.DepositAmount = request.Type == 0 ? request.Amount : 0;
        cashRegisterDetail.WithdrawalAmount = request.Type == 1 ? request.Amount : 0;

        cashRegisterDetail.Description = request.Description;
        cashRegisterDetail.Date = request.Date;

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return DomainResult.Success("Kasa hareketi başarıyla güncellendi");
    }
}