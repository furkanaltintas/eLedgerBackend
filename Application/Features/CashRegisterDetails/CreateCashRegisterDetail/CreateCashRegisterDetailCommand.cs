using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.CreateCashRegisterDetail;

public record CreateCashRegisterDetailCommand(
    Guid CashRegisterId,
    DateOnly Date,
    int Type,
    decimal Amount,
    Guid? OppositeCashRegisterId,
    Guid? OppositeBankId,
    Guid? OppositeCustomerId,
    decimal OppositeAmount,
    string Description) : IRequest<IDomainResult<string>>;
