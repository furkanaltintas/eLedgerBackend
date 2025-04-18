using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.CashRegisters.Commands;
using Application.Features.CashRegisters.Constants;
using Application.Features.CashRegisters.Rules;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.CashRegisters.Handlers;

class DeleteCashRegisterCommandHandler : BaseCommandHandler, IRequestHandler<DeleteCashRegisterCommand, IDomainResult<string>>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly CashRegisterRules _cashRegisterRules;

    public DeleteCashRegisterCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, ICashRegisterRepository cashRegisterRepository, CashRegisterRules cashRegisterRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _cashRegisterRepository = cashRegisterRepository;
        _cashRegisterRules = cashRegisterRules;
    }

    public async Task<IDomainResult<string>> Handle(DeleteCashRegisterCommand request, CancellationToken cancellationToken)
    {
        CashRegister? cashRegister = await _cashRegisterRules.CheckAsync(request.Id, cancellationToken);
        if (cashRegister is null) return DomainResult.Failed<string>(CashRegistersMessages.NotFound);

        cashRegister.IsDeleted = true;
        return await Success(new[] { CashRegistersMessages.Cache }, CashRegistersMessages.Deleted, cancellationToken);
    }
}