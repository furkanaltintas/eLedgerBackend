using Application.Common.Handlers.Partners;
using Application.Common.Interfaces;
using Application.Features.Users.Constants;
using Application.Features.Users.Queries;
using Domain.Entities.Partners;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Handlers;

internal sealed class GetAllUsersQueryHandler : ApplicationCacheableQueryHandlerBase, IRequestHandler<GetAllUsersQuery, IDomainResult<List<AppUser>>>
{
    private readonly UserManager<AppUser> _userManager;
    public GetAllUsersQueryHandler(ICacheService cacheService, UserManager<AppUser> userManager) : base(cacheService)
    {
        _userManager = userManager;
    }

    public async Task<IDomainResult<List<AppUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<AppUser> users = await GetOrSetCacheAsync(UsersMessages.Cache, async () =>
        {
            return await _userManager.Users.Include(u => u.CompanyUsers)!
                                           .ThenInclude(cu => cu.Company)
                                           .OrderBy(u => u.FirstName)
                                           .ThenBy(u => u.LastName)
                                           .ToListAsync(cancellationToken);
        });

        return DomainResult.Success(users);
    }
}