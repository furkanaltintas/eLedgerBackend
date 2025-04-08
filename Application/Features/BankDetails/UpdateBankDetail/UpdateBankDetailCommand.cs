using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.UpdateBankDetail;

public record UpdateBankDetailCommand(
    Guid Id,
    Guid CashRegisterId,
    int Type,
    decimal Amount,
    string Description,
    DateOnly Date) : IRequest<IDomainResult<string>>;