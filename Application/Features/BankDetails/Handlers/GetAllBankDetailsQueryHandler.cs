using Application.Features.BankDetails.Queries;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.BankDetails.Handlers;

class GetAllBankDetailsQueryHandler(
    IBankRepository bankRepository) : IRequestHandler<GetAllBankDetailsQuery, IDomainResult<Bank>>
{
    public async Task<IDomainResult<Bank>> Handle(GetAllBankDetailsQuery request, CancellationToken cancellationToken)
    {
        Bank? bank = await bankRepository
            .Where(c => c.Id == request.BankId)
            .Include(c => c.Details!.Where(crd => crd.Date >= request.StartDate && crd.Date <= request.EndDate))
            .FirstOrDefaultAsync(cancellationToken);

        if (bank is null) return DomainResult<Bank>.NotFound("Cash register not found.");
        return DomainResult.Success(bank);
    }
}