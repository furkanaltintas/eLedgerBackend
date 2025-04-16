using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.Commands;

public sealed record DeleteCustomerCommand(Guid Id) : IRequest<IDomainResult<string>>;