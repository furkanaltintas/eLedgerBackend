using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CashRegisters.GetAllCashRegisters;

class GetAllCashRegistersQueryHandler(
    ICashRegisterRepository cashRegisterRepository,
    ICacheService cacheService) : IRequestHandler<GetAllCashRegistersQuery, IDomainResult<List<CashRegister>>>
{
    public async Task<IDomainResult<List<CashRegister>>> Handle(GetAllCashRegistersQuery request, CancellationToken cancellationToken)
    {
        List<CashRegister> cashRegisters;

        cashRegisters = cacheService.Get<List<CashRegister>>("cashRegisters");
        if (cashRegisters is null)
        {
            cashRegisters = await cashRegisterRepository
                .GetAll()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            cacheService.Set("cashRegisters", cashRegisters);
        }

        return DomainResult.Success(cashRegisters);
    }
}