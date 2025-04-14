using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.Commands;

public record MigrateAllCompaniesCommand() : IRequest<IDomainResult<string>>;