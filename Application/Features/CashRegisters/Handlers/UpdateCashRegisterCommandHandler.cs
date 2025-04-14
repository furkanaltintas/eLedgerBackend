using Application.Common.Handlers;
using Application.Common.Interfaces;
using Application.Features.CashRegisters.Commands;
using Application.Features.CashRegisters.Constants;
using Application.Features.CashRegisters.Rules;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MapsterMapper;
using MediatR;

namespace Application.Features.CashRegisters.Handlers;

class UpdateCashRegisterCommandHandler : BaseCommandHandler, IRequestHandler<UpdateCashRegisterCommand, IDomainResult<string>>
{
    private readonly CashRegisterRules _cashRegisterRules;

    public UpdateCashRegisterCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, CashRegisterRules cashRegisterRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _cashRegisterRules = cashRegisterRules;
    }

    public async Task<IDomainResult<string>> Handle(UpdateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        CashRegister? cashRegister = await _cashRegisterRules.CheckAsync(request.Id, cancellationToken);
        if (cashRegister is null) return DomainResult.Failed<string>(CashRegistersMessages.NotFound);

        if (cashRegister.Name != request.Name)
        {
            IDomainResult<string> nameCheckResult = await _cashRegisterRules.CheckNameExistsAsync(request.Name, cancellationToken);
            if (!nameCheckResult.IsSuccess) return DomainResult.Failed<string>(CashRegistersMessages.AlreadyNameExists);
        }

        Mapper.Map(request, cashRegister);

        return await Success(new[] { CashRegistersMessages.Cache }, CashRegistersMessages.Updated, cancellationToken);
    }
}