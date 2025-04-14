using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.Commands;

public record UpdateCashRegisterCommand(
    Guid Id,
    string Name,
    int CurrencyTypeValue) : IRequest<IDomainResult<string>>;
