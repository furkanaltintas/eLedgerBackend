using Application.Common.Interfaces;
using Application.Features.Users.Commands;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Handlers;

class UpdateUserCommandHandler(
    UserManager<AppUser> userManager,
    ICompanyUserRepository companyUserRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<UpdateUserCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users
            .Where(u => u.Id == request.Id)
            .Include(u => u.CompanyUsers)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null) return DomainResult<string>.Failed("User not found");

        if (user.UserName != request.UserName)
        {
            bool isUserNameExists = await userManager.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken);
            if (isUserNameExists) return DomainResult<string>.Failed("User already exists");
        }

        if (user.Email != request.Email)
        {
            bool isEmailExists = await userManager.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (isEmailExists) return DomainResult<string>.Failed("Email already exists");

            user.EmailConfirmed = false;
        }

        mapper.Map(request, user);

        IdentityResult identityResult = await userManager.UpdateAsync(user);
        if (!identityResult.Succeeded) return DomainResult<string>.Failed(identityResult.Errors.Select(e => e.Description).ToList());

        if (request.Password is not null)
        {
            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            identityResult = await userManager.ResetPasswordAsync(user, token, request.Password);
            if (!identityResult.Succeeded) return DomainResult<string>.Failed(identityResult.Errors.Select(e => e.Description).ToList());
        }

        if (request.CompanyIds!.Any())
        {
            companyUserRepository.DeleteRange(user.CompanyUsers);

            List<CompanyUser> companyUsers = request.CompanyIds!.Select(companyId => new CompanyUser
            {
                CompanyId = Guid.Parse(companyId),
                AppUserId = user.Id
            }).ToList();

            await companyUserRepository.AddRangeAsync(companyUsers);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        cacheService.Remove("users");

        return DomainResult.Success("User updated successfully");
    }
}