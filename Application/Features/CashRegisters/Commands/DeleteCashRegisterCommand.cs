using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisters.Commands;

public record DeleteCashRegisterCommand(Guid Id) : IRequest<IDomainResult<string>>;