using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<IDomainResult<string>>;