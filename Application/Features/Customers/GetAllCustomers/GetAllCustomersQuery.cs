using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.GetAllCustomers;

public record GetAllCustomersQuery() : IRequest<IDomainResult<List<Customer>>>;