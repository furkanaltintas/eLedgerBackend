using Application.Features.Auth.Login;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Auth.ChangeCompany;

public record ChangeCompanyCommand(Guid CompanyId) : IRequest<IDomainResult<LoginCommandResponse>>;