using Domain.Entities;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetAllUsers;

class GetAllUsersQueryHandler(UserManager<AppUser> userManager) : IRequestHandler<GetAllUsersQuery, IDomainResult<List<AppUser>>>
{
    public async Task<IDomainResult<List<AppUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<AppUser> users = await userManager.Users
            .Include(u => u.CompanyUsers)!
            .ThenInclude(cu => cu.Company)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);

        return DomainResult.Success(users);
    }
}