using Application.Common.Handlers.Partners;
using Application.Common.Interfaces;
using Application.Features.Users.Commands;
using Application.Features.Users.Constants;
using Application.Features.Users.Rules;
using Domain.Entities.Partners;
using Domain.Interfaces;
using DomainResults.Common;
using GenericRepository;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Handlers;

internal sealed class UpdateUserCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<UpdateUserCommand, IDomainResult<string>>
{
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly UserRules _userRules;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, IMapper mapper, ICompanyUserRepository companyUserRepository, UserManager<AppUser> userManager, UserRules userRules) : base(unitOfWork, cacheService, mapper)
    {
        _companyUserRepository = companyUserRepository;
        _userManager = userManager;
        _userRules = userRules;
    }

    public async Task<IDomainResult<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await _userRules.GetUserWithCompaniesAsync(request.Id, cancellationToken);
        if (user is null) return DomainResult<string>.Failed(UsersMessages.NotFound);

        IDomainResult<string> checkUserNameExistsResult = await _userRules.CheckUserNameExistsAsync(user.UserName!, request.UserName, cancellationToken);
        if(!checkUserNameExistsResult.IsSuccess) return checkUserNameExistsResult;

        IDomainResult<string> checkEmailExistsResult = await _userRules.CheckEmailExistsAsync(request.Email, user, cancellationToken);
        if (!checkEmailExistsResult.IsSuccess) return checkEmailExistsResult;

        Mapper.Map(request, user);

        IDomainResult<string> updateUserResult = await UpdateUserAsync(user, cancellationToken);
        if (!updateUserResult.IsSuccess) return updateUserResult;

        IDomainResult<string> updatePasswordResult = await UpdatePasswordAsync(user, request.Password, cancellationToken);
        if (!updatePasswordResult.IsSuccess) return updatePasswordResult;

        await UpdateUserCompanyRelationsAsync(user, request.CompanyIds, cancellationToken);
        CacheService.Remove(UsersMessages.Cache);
        return DomainResult.Success(UsersMessages.Updated);
    }

    private async Task<IDomainResult<string>> UpdateUserAsync(AppUser user, CancellationToken cancellationToken)
    {
        var identityResult = await _userManager.UpdateAsync(user);
        return identityResult.Succeeded
            ? DomainResult.Success(String.Empty)
            : DomainResult.Failed<string>(identityResult.Errors.Select(e => e.Description).ToList());
    }

    private async Task<IDomainResult<string>> UpdatePasswordAsync(AppUser user, string? newPassword, CancellationToken cancellationToken)
    {
        if (newPassword is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return identityResult.Succeeded
                ? DomainResult.Success(String.Empty)
                : DomainResult.Failed<string>(identityResult.Errors.Select(e => e.Description).ToList());
        }

        return DomainResult.Success(String.Empty);
    }

    private async Task UpdateUserCompanyRelationsAsync(AppUser user, List<string> companyIds, CancellationToken cancellationToken)
    {
        if (companyIds is null || !companyIds.Any()) return;

        _companyUserRepository.DeleteRange(user.CompanyUsers);

        var companyUsers = companyIds.Select(companyId => new CompanyUser
        {
            CompanyId = Guid.Parse(companyId),
            AppUserId = user.Id
        }).ToList();

        await _companyUserRepository.AddRangeAsync(companyUsers);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

}