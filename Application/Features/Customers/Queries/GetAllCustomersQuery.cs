using Domain.Entities.Companies;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.Queries;

public sealed record GetAllCustomersQuery() : IRequest<IDomainResult<List<Customer>>>;