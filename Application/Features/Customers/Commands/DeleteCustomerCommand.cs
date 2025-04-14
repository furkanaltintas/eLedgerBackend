using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.Commands;

public record DeleteCustomerCommand(Guid Id) : IRequest<IDomainResult<string>>;