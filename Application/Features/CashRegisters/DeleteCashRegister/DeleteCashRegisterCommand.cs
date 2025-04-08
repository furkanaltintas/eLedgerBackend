using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.DeleteCashRegister;

public record DeleteCashRegisterCommand(Guid Id): IRequest<IDomainResult<string>>;