using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Commands;

public record DeleteCompanyCommand(Guid Id) : IRequest<IDomainResult<string>>;