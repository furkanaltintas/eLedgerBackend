using Domain.ValueObjects;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.CreateCompany;

public record CreateCompanyCommand(
    string Name,
    string FullAddress,
    string TaxDepartment,
    string TaxNumber,
    Database Database): IRequest<IDomainResult<string>>;