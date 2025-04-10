using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.CustomerDetails.GetAllCustomerDetails;

public sealed record GetAllCustomerDetailsQuery(Guid CustomerId): IRequest<IDomainResult<Customer>>;