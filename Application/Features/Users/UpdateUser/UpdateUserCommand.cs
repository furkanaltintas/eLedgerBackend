using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Password,
    List<string> CompanyIds) : IRequest<IDomainResult<string>>;