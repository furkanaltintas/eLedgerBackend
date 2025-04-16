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

internal sealed class CreateUserCommandHandler : ApplicationCommandHandlerBase, IRequestHandler<CreateUserCommand, IDomainResult<string>>
{
    private readonly ICompanyUserRepository _companyUserRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly UserRules _userRules;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, IMapper mapper, ICompanyUserRepository companyUserRepository, UserManager<AppUser> userManager, UserRules userRules) : base(unitOfWork, cacheService, mapper)
    {
        _companyUserRepository = companyUserRepository;
        _userManager = userManager;
        _userRules = userRules;
    }

    public async Task<IDomainResult<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        IDomainResult<String> checkUserExistsResult = await _userRules.CheckUserExistsByUserNameOrEmailAsync(request.UserName, request.Email, cancellationToken);
        if (!checkUserExistsResult.IsSuccess) return checkUserExistsResult;

        AppUser user = Mapper.Map<AppUser>(request);

        IdentityResult identityResult = await _userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded) return DomainResult<string>.Failed(identityResult.Errors.Select(e => e.Description).ToList());

        await CompanyUsersAsync(request, user, cancellationToken);
        return await SuccessAsync(UsersMessages.Cache, UsersMessages.Created, cancellationToken);
    }

    private async Task CompanyUsersAsync(CreateUserCommand request, AppUser user, CancellationToken cancellationToken)
    {
        List<CompanyUser> companyUsers = request.CompanyIds.Select(companyId => new CompanyUser
        {
            CompanyId = Guid.Parse(companyId),
            AppUserId = user.Id
        }).ToList();

        await _companyUserRepository.AddRangeAsync(companyUsers);
    }
}
