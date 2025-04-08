using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.CreateCashRegisterDetail;

public record CreateCashRegisterDetailCommand(
    Guid? OppositeCashRegisterId,
    Guid CashRegisterId,
    DateOnly Date,
    int Type,
    decimal Amount,
    decimal OppositeAmount,
    string Description) : IRequest<IDomainResult<string>>;
