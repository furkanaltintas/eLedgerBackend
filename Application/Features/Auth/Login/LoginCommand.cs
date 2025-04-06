using DomainResults.Common;
using Infrastructure.Configurations;
using MediatR;

namespace Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<IDomainResult<LoginCommandResponse>>;