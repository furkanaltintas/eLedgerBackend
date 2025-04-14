using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.Commands;

public record CreateCashRegisterCommand(
    string Name,
    int CurrencyTypeValue) : IRequest<IDomainResult<string>>;