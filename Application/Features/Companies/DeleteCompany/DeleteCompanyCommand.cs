using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.DeleteCompany;

public record DeleteCompanyCommand(Guid Id) : IRequest<IDomainResult<string>>;