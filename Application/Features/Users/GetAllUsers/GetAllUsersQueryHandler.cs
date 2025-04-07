using Domain.Entities;
using DomainResults.Common;
using Infrastructure.Services.Cache;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.GetAllUsers;

class GetAllUsersQueryHandler(
    UserManager<AppUser> userManager,
    ICacheService cacheService) : IRequestHandler<GetAllUsersQuery, IDomainResult<List<AppUser>>>
{
    public async Task<IDomainResult<List<AppUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<AppUser> users;

        users = cacheService.Get<List<AppUser>>("users");
        if(users is null)
        {
            users = await userManager.Users
                .Include(u => u.CompanyUsers)!
                .ThenInclude(cu => cu.Company)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ToListAsync(cancellationToken);

            cacheService.Set("users", users);
        }

        return DomainResult.Success(users);
    }
}