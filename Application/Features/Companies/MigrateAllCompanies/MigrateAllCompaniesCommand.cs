using DomainResults.Common;
using MediatR;

namespace Application.Features.Companies.MigrateAllCompanies;

public record MigrateAllCompaniesCommand() : IRequest<IDomainResult<string>>;