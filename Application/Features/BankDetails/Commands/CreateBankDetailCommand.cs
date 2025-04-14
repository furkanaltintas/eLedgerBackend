using DomainResults.Common;
using MediatR;

namespace Application.Features.BankDetails.Commands;

public record CreateBankDetailCommand(
    Guid BankId,
    DateOnly Date,
    int Type,
    decimal Amount,
    Guid? OppositeBankId,
    Guid? OppositeCashRegisterId,
    Guid? OppositeCustomerId,
    decimal OppositeAmount,
    string Description) : IRequest<IDomainResult<string>>;