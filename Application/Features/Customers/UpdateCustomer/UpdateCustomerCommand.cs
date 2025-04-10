﻿using DomainResults.Common;
using MediatR;

namespace Application.Features.Customers.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    int TypeValue,
    string City,
    string Town,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber) : IRequest<IDomainResult<string>>;