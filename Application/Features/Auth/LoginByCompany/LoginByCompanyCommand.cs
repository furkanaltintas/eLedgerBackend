using DomainResults.Common;
using Infrastructure.Configurations;
using MediatR;

namespace Application.Features.Auth.LoginByCompany;

public record LoginByCompanyCommand(string UserId, string CompanyId) : IRequest<IDomainResult<LoginCommandResponse>>;