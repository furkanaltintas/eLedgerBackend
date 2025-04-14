using Application.Features.Banks.Constants;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;

namespace Application.Features.Banks.Rules;

public class BankRules(IBankRepository bankRepository)
{
    public async Task<IDomainResult<string>> CheckIBANExistsAsync(string iban, CancellationToken cancellationToken)
    {
        bool isIBANExists = await bankRepository.AnyAsync(b => b.IBAN == iban, cancellationToken);
        return isIBANExists
            ? DomainResult.Failed<string>(BanksMessages.AlreadyIBANExists)
            : DomainResult.Success(string.Empty);
    }

    public async Task<Bank> CheckAsync(Guid bankId, CancellationToken cancellationToken)
    {
        return await bankRepository.GetByExpressionWithTrackingAsync(b => b.Id == bankId, cancellationToken);
    }
}