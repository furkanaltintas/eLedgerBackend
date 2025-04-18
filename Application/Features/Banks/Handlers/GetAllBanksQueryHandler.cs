using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.Banks.Constants;
using Application.Features.Banks.Queries;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Banks.Handlers;

class GetAllBanksQueryHandler : BaseQueryHandler, IRequestHandler<GetAllBanksQuery, IDomainResult<List<Bank>>>
{
    private readonly IBankRepository _bankRepository;
    public GetAllBanksQueryHandler(ICompanyContextHelper companyContextHelper, IMapper mapper, IBankRepository bankRepository) : base(companyContextHelper, mapper)
    {
        _bankRepository = bankRepository;
    }

    public async Task<IDomainResult<List<Bank>>> Handle(GetAllBanksQuery request, CancellationToken cancellationToken)
    {
        List<Bank> banks = await GetOrSetCacheAsync(BanksMessages.Cache, async () => 
        await _bankRepository.GetAll().OrderBy(b => b.Name).ToListAsync(cancellationToken));
        return DomainResult.Success(banks);
    }
}