using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Commands;

public sealed record DeleteCompanyCommand(Guid Id) : IRequest<IDomainResult<string>>;