using DomainResults.Common;
using Infrastructure.Configurations;
using MediatR;

namespace Application.Features.Auth.ChangeCompany;

public record ChangeCompanyCommand(Guid CompanyId) : IRequest<IDomainResult<LoginCommandResponse>>;