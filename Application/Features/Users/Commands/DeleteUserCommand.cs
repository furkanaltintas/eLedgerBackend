using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest<IDomainResult<string>>;