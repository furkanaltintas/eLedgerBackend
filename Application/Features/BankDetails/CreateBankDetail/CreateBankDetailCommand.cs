using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.CreateBankDetail;

public record CreateBankDetailCommand(
    Guid? OppositeBankId,
    Guid BankId,
    DateOnly Date,
    int Type,
    decimal Amount,
    decimal OppositeAmount,
    string Description) : IRequest<IDomainResult<string>>;