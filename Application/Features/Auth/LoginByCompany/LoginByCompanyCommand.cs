using Application.Features.Auth.Login;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Auth.LoginByCompany;

public record LoginByCompanyCommand(string UserId, string CompanyId) : IRequest<IDomainResult<LoginCommandResponse>>;