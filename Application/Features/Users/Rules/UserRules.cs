using Application.Features.Users.Constants;
using Domain.Entities.Partners;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Rules;

public class UserRules(UserManager<AppUser> userManager)
{
    public async Task<IDomainResult<string>> CheckUserNameExistsAsync(string currentUserName, string newUserName, CancellationToken cancellationToken)
    {
        if (!string.Equals(currentUserName, newUserName, StringComparison.OrdinalIgnoreCase))
        {
            Boolean isUserNameExists = await userManager.Users.AnyAsync(u => u.UserName == newUserName, cancellationToken);
            if (isUserNameExists) return DomainResult<string>.Failed(UsersMessages.UserNameAlreadyExists);
        }

        return DomainResult.Success(string.Empty);
    }

    public async Task<IDomainResult<string>> CheckEmailExistsAsync(string newEmail, AppUser user, CancellationToken cancellationToken)
    {
        if (!string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
        {
            Boolean isEmailExists = await userManager.Users.AnyAsync(u => u.Email == newEmail, cancellationToken);
            if (isEmailExists) return DomainResult<string>.Failed(UsersMessages.MailAlreadyExists);

            user.EmailConfirmed = false;
        }

        return DomainResult.Success(string.Empty);
    }

    public async Task<IDomainResult<string>> CheckUserExistsByUserNameOrEmailAsync(string userName, string email, CancellationToken cancellationToken)
    {
        Boolean isUserExists = await userManager.Users
            .AnyAsync(u => u.UserName == userName || u.Email == email, cancellationToken);

        return isUserExists
            ? DomainResult<string>.Failed(UsersMessages.UserAlreadyExists)
            : DomainResult.Success(string.Empty);
    }



    public async Task<AppUser> GetUserWithCompaniesAsync(Guid userId, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users
                                         .Where(u => u.Id == userId)
                                         .Include(u => u.CompanyUsers)
                                         .FirstOrDefaultAsync(cancellationToken);
        return user ?? null!;
    }

    public async Task<AppUser> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(userId.ToString());
        return user ?? null!;
    }
}