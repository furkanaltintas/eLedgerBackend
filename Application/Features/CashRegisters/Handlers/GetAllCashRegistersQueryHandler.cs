using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.CashRegisters.Constants;
using Application.Features.CashRegisters.Queries;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CashRegisters.Handlers;

class GetAllCashRegistersQueryHandler : BaseQueryHandler, IRequestHandler<GetAllCashRegistersQuery, IDomainResult<List<CashRegister>>>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    public GetAllCashRegistersQueryHandler(ICompanyContextHelper companyContextHelper, IMapper mapper, ICashRegisterRepository cashRegisterRepository) : base(companyContextHelper, mapper)
    {
        _cashRegisterRepository = cashRegisterRepository;
    }

    public async Task<IDomainResult<List<CashRegister>>> Handle(GetAllCashRegistersQuery request, CancellationToken cancellationToken)
    {
        List<CashRegister> cashRegisters = await GetOrSetCacheAsync(CashRegistersMessages.Cache, async () =>
        await _cashRegisterRepository.GetAll().OrderBy(c => c.Name).ToListAsync(cancellationToken));
        return DomainResult.Success(cashRegisters);
    }
}