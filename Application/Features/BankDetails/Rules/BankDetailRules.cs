using Domain.Entities;
using Domain.Interfaces;

namespace Application.Features.BankDetails.Rules;

public class BankDetailRules(IBankDetailRepository bankDetailRepository)
{
    public async Task<BankDetail> CheckAsync(Guid id, CancellationToken cancellationToken)
    {
        BankDetail? bankDetail = await bankDetailRepository.GetByExpressionWithTrackingAsync(c => c.Id == id, cancellationToken);
        return bankDetail;
    }
}