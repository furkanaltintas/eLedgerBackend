using DomainResults.Common;
using MediatR;

namespace Application.Features.CashRegisterDetails.DeleteCashRegisterDetail;

public record DeleteCashRegisterDetailCommand(Guid Id) : IRequest<IDomainResult<string>>;