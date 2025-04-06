using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password,
    List<string> CompanyIds) : IRequest<IDomainResult<string>>;