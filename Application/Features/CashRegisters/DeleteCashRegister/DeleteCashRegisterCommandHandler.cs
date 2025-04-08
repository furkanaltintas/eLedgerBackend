using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using Infrastructure.Services.Cache;
using MediatR;

namespace Application.Features.CashRegisters.DeleteCashRegister;

class DeleteCashRegisterCommandHandler(
    ICashRegisterRepository cashRegisterRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService) : IRequestHandler<DeleteCashRegisterCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteCashRegisterCommand request, CancellationToken cancellationToken)
    {
        CashRegister? cashRegister = await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == request.Id, cancellationToken);
        if (cashRegister is null) return DomainResult.Failed<string>("Cash register not found.");


        cashRegister.IsDeleted = true;
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return DomainResult.Success("Cash register deleted successfully.");
    }
}