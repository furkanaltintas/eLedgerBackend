using Application.Common.Interfaces;
using Application.Features.Users.Commands;
using Application.Features.Users.Constants;
using Application.Features.Users.Rules;
using Domain.Entities.Partners;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Handlers;

internal sealed class DeleteUserCommandHandler(
    UserManager<AppUser> userManager,
    ICacheService cacheService,
    UserRules userRules) : IRequestHandler<DeleteUserCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userRules.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) return DomainResult<string>.NotFound(UsersMessages.NotFound);
        user.IsDeleted = true;

        IdentityResult identityResult = await userManager.UpdateAsync(user);
        if (!identityResult.Succeeded) return DomainResult<string>.Failed(identityResult.Errors.Select(e => e.Description).ToList());

        cacheService.Remove(UsersMessages.Cache);
        return DomainResult.Success(UsersMessages.Deleted);
    }
}