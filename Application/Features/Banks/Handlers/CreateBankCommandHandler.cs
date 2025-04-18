using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.Banks.Commands;
using Application.Features.Banks.Constants;
using Application.Features.Banks.Rules;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.Banks.Handlers;

class CreateBankCommandHandler : BaseCommandHandler, IRequestHandler<CreateBankCommand, IDomainResult<string>>
{
    private readonly IBankRepository _bankRepository;
    private readonly BankRules _bankRules;
    public CreateBankCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, IBankRepository bankRepository, BankRules bankRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _bankRepository = bankRepository;
        _bankRules = bankRules;
    }

    public async Task<IDomainResult<string>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
    {
        IDomainResult<string> ibanCheckResult = await _bankRules.CheckIBANExistsAsync(request.IBAN, cancellationToken); // IBAN kontrolü
        if (!ibanCheckResult.IsSuccess) return ibanCheckResult;

        Bank bank = Mapper.Map<Bank>(request);
        await _bankRepository.AddAsync(bank, cancellationToken);
        return await Success(new[] { BanksMessages.Cache }, BanksMessages.Created, cancellationToken);
    }
}

#region Success yapısı ile bu işlemlere gerek kalmadı
// await _unitOfWorkCompany.SaveChangesAsync(cancellationToken);
// _companyContextHelper.RemoveCompanyFromContext(BanksMessages.Cache);
// return DomainResult.Success(BanksMessages.Created);
#endregion