using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Commands;

public sealed record MigrateAllCompaniesCommand() : IRequest<IDomainResult<string>>;