using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CashRegisters.GetAllCashRegisters;

class GetAllCashRegistersQueryHandler(
    ICashRegisterRepository cashRegisterRepository,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<GetAllCashRegistersQuery, IDomainResult<List<CashRegister>>>
{
    public async Task<IDomainResult<List<CashRegister>>> Handle(GetAllCashRegistersQuery request, CancellationToken cancellationToken)
    {
        List<CashRegister> cashRegisters;

        cashRegisters = companyContextHelper.GetCompanyFromContext<List<CashRegister>>("cashRegisters");
        if (cashRegisters is null)
        {
            cashRegisters = await cashRegisterRepository
                .GetAll()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            companyContextHelper.SetCompanyInContext("cashRegisters", cashRegisters);
        }

        return DomainResult.Success(cashRegisters);
    }
}