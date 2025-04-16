using Domain.Entities.Partners;
using DomainResults.Common;
using MediatR;

namespace Application.Features.Users.Queries;

public sealed record GetAllUsersQuery() : IRequest<IDomainResult<List<AppUser>>>;