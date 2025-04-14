using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.Commands;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    List<string> CompanyIds,
    bool IsAdmin) : IRequest<IDomainResult<string>>;