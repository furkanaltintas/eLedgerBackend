using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.Commands;

public record UpdateCashRegisterDetailCommand(
    Guid Id,
    Guid CashRegisterId,
    int Type,
    decimal Amount,
    string Description,
    DateOnly Date) : IRequest<IDomainResult<string>>;