using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.UpdateCashRegister;

public record UpdateCashRegisterCommand(
    Guid Id,
    string Name,
    int TypeValue) : IRequest<IDomainResult<string>>;
