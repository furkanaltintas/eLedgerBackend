using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using Infrastructure.Services.Cache;
using MediatR;

namespace Application.Features.CashRegisterDetails.CreateCashRegisterDetail;

class CreateCashRegisterDetailCommandHandler(
    ICashRegisterDetailRepository cashRegisterDetailRepository,
    ICashRegisterRepository cashRegisterRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<CreateCashRegisterDetailCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateCashRegisterDetailCommand request, CancellationToken cancellationToken)
    {
        CashRegister cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.CashRegisterId, cancellationToken);
        cashRegister.DepositAmount += (request.Type == 0 ? request.Amount : 0);
        cashRegister.WithdrawalAmount += (request.Type == 1 ? request.Amount : 0);

        CashRegisterDetail cashRegisterDetail = new()
        {
            Date = request.Date,
            DepositAmount = request.Type == 0 ? request.Amount : 0,
            WithdrawalAmount = request.Type == 1 ? request.Amount : 0,
            Description = request.Description,
            CashRegisterId = request.CashRegisterId,
        };

        await cashRegisterDetailRepository.AddAsync(cashRegisterDetail, cancellationToken);

        if (request.OppositeCashRegisterId is not null)
        {
            CashRegister oppositeCashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.OppositeCashRegisterId, cancellationToken);
            oppositeCashRegister.DepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
            oppositeCashRegister.WithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

            CashRegisterDetail oppositeCashRegisterDetail = new()
            {
                Date = request.Date,
                DepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                WithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                CashRegisterDetailId = cashRegisterDetail.CashRegisterDetailId,
                Description = request.Description,
                CashRegisterId = (Guid)request.OppositeCashRegisterId,
            };

            cashRegisterDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

            await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);
        }

        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return DomainResult.Success("Cash register detail created successfully.");
    }
}