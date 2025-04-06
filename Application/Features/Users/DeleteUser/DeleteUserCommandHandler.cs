using Domain.Entities;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.DeleteUser;

class DeleteUserCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<DeleteUserCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null) return DomainResult<string>.NotFound("User not found");

        user.IsDeleted = true;

        IdentityResult identityResult = await userManager.UpdateAsync(user);
        if (!identityResult.Succeeded) return DomainResult<string>.Failed(identityResult.Errors.Select(e => e.Description).ToList());

        return DomainResult.Success("User deleted successfully");
    }
}

