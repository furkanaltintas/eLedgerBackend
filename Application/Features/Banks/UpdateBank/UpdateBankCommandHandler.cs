using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Banks.UpdateBank;

class UpdateBankCommandHandler(
    IBankRepository bankRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<UpdateBankCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
    {
        Bank bank = await bankRepository.GetByExpressionWithTrackingAsync(b => b.Id == request.Id, cancellationToken);
        if (bank is null) return DomainResult.NotFound<string>($"Bank not found.");

        if (bank.IBAN != request.IBAN)
        {
            Boolean isIBANExists = await bankRepository.AnyAsync(b => b.IBAN == request.IBAN, cancellationToken);
            if (isIBANExists) return DomainResult.Failed<string>("IBAN already exists");
        }

        mapper.Map(request, bank);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        companyContextHelper.RemoveCompanyFromContext("banks");
        return DomainResult.Success("Bank updated successfully");
    }
}