using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Banks.CreateBank;

class CreateBankCommandHandler(
    IBankRepository bankRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    IMapper mapper,
    ICompanyContextHelper companyContextHelper) : IRequestHandler<CreateBankCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        Boolean isIBANExists = await bankRepository.AnyAsync(b => b.IBAN == request.IBAN, cancellationToken);
        if (isIBANExists) return DomainResult.Failed<string>("IBAN already exists");

        Bank bank = mapper.Map<Bank>(request);

        await bankRepository.AddAsync(bank, cancellationToken);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);
        companyContextHelper.RemoveCompanyFromContext("banks");
        return DomainResult.Success("Bank created successfully");
    }
}

