using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Banks.GetAllBanks;

class GetAllBanksQueryHandler(
    IBankRepository bankRepository,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<GetAllBanksQuery, IDomainResult<List<Bank>>>
{
    public async Task<IDomainResult<List<Bank>>> Handle(GetAllBanksQuery request, CancellationToken cancellationToken)
    {
        List<Bank> banks;

        banks = companyContextHelper.GetCompanyFromContext<List<Bank>>("banks");
        if (banks is null)
        {
            banks = await bankRepository.GetAll().OrderBy(b => b.Name).ToListAsync(cancellationToken);
            companyContextHelper.SetCompanyInContext("banks", banks);
        }
        return DomainResult.Success(banks);
    }
}