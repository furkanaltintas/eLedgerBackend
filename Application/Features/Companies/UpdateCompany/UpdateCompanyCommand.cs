using Domain.ValueObjects;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.UpdateCompany;

public record UpdateCompanyCommand(
    Guid Id,
    string Name,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber,
    Database Database) : IRequest<IDomainResult<string>>;