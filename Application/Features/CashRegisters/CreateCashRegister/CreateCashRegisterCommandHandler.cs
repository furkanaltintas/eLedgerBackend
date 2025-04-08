using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using Infrastructure.Services.Cache;
using MapsterMapper;
using MediatR;

namespace Application.Features.CashRegisters.CreateCashRegister;

class CreateCashRegisterCommandHandler(
    ICashRegisterRepository cashRegisterRepository,
    IUnitOfWorkCompany unitOfWorkCompany,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<CreateCashRegisterCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        Boolean isNameExists = await cashRegisterRepository.AnyAsync(c => c.Name == request.Name, cancellationToken);
        if(isNameExists) return DomainResult.Failed<string>("Bu isimde bir kasa zaten mevcut.");

        CashRegister cashRegister = mapper.Map<CashRegister>(request);

        await cashRegisterRepository.AddAsync(cashRegister, cancellationToken);
        await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

        cacheService.Remove("cashRegisters");

        return DomainResult.Success("Kasa kaydı başarıyla tamamlandı.");
    }
}
