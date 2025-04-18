using Application.Features.CashRegisters.Constants;
using Domain.Entities.Companies;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.Rules;

public class CashRegisterRules(ICashRegisterRepository cashRegisterRepository)
{
    public async Task<IDomainResult<string>> CheckNameExistsAsync(string name, CancellationToken cancellationToken)
    {
        bool isNameExists = await cashRegisterRepository.AnyAsync(b => b.Name == name, cancellationToken);
        return isNameExists
            ? DomainResult.Failed<string>(CashRegistersMessages.AlreadyNameExists)
            : DomainResult.Success(string.Empty);
    }

    public async Task<CashRegister> CheckAsync(Guid cashRegisterId, CancellationToken cancellationToken)
    {
        return await cashRegisterRepository.GetByExpressionWithTrackingAsync(c => c.Id == cashRegisterId, cancellationToken);
    }
}