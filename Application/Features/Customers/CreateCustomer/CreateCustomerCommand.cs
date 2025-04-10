using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.CreateCustomer;

public record CreateCustomerCommand(
    string Name,
    int TypeValue,
    string City,
    string Town,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber) : IRequest<IDomainResult<string>>;