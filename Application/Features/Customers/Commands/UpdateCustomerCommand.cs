using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.Commands;

public sealed record UpdateCustomerCommand(
    Guid Id,
    string Name,
    int TypeValue,
    string City,
    string Town,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber) : IRequest<IDomainResult<string>>;