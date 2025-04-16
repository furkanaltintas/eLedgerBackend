using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.Commands;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    List<string> CompanyIds,
    bool IsAdmin) : IRequest<IDomainResult<string>>;