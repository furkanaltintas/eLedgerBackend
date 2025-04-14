using Application.Features.CashRegisterDetails.Queries;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CashRegisterDetails.Handlers;

class GetAllCashRegisterDetailsQueryHandler(ICashRegisterRepository cashRegisterRepository) : IRequestHandler<GetAllCashRegisterDetailsQuery, IDomainResult<CashRegister>>
{
    public async Task<IDomainResult<CashRegister>> Handle(GetAllCashRegisterDetailsQuery request, CancellationToken cancellationToken)
    {
        CashRegister? cashRegister = await cashRegisterRepository
            .Where(c => c.Id == request.CashRegisterId)
            .Include(c => c.Details!.Where(crd => crd.Date >= request.StartDate && crd.Date <= request.EndDate))
            .FirstOrDefaultAsync(cancellationToken);

        if (cashRegister is null) return DomainResult<CashRegister>.NotFound("Cash register not found.");
        return DomainResult.Success(cashRegister);
    }
}