using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.Commands;

public sealed record DeleteUserCommand(Guid Id) : IRequest<IDomainResult<string>>;