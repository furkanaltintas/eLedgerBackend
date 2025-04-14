using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.Commands;

public record DeleteCashRegisterDetailCommand(Guid Id) : IRequest<IDomainResult<string>>;