using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.DeleteCustomer;

public record DeleteCustomerCommand(Guid Id) : IRequest<IDomainResult<string>>;