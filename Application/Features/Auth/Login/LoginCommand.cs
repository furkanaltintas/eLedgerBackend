using DomainResults.Common;
using MediatR;

namespace Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<IDomainResult<LoginCommandResponse>>;