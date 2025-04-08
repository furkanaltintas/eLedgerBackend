using Domain.Entities;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.GetAllUsers;

public record GetAllUsersQuery() : IRequest<IDomainResult<List<AppUser>>>;