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

class CreateCashRegisterCommandHandler : BaseCommandHandler, IRequestHandler<CreateCashRegisterCommand, IDomainResult<string>>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly CashRegisterRules _cashRegisterRules;

    public CreateCashRegisterCommandHandler(IUnitOfWorkCompany unitOfWorkCompany, ICompanyContextHelper companyContextHelper, IMapper mapper, ICashRegisterRepository cashRegisterRepository, CashRegisterRules cashRegisterRules) : base(unitOfWorkCompany, companyContextHelper, mapper)
    {
        _cashRegisterRepository = cashRegisterRepository;
        _cashRegisterRules = cashRegisterRules;
    }

    public async Task<IDomainResult<string>> Handle(CreateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        //bool isNameExists = await _cashRegisterRepository.AnyAsync(c => c.Name == request.Name, cancellationToken);
        //if (isNameExists) return DomainResult.Failed<string>("Bu isimde bir kasa zaten mevcut.");

        IDomainResult<string> result = await _cashRegisterRules.CheckNameExistsAsync(request.Name, cancellationToken);
        if (!result.IsSuccess) return result;

        CashRegister cashRegister = Mapper.Map<CashRegister>(request);

        await _cashRegisterRepository.AddAsync(cashRegister, cancellationToken);
        return await Success(new[] { CashRegistersMessages.Cache }, CashRegistersMessages.Created, cancellationToken);
    }
}