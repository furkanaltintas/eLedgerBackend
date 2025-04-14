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

class CreateUserCommandHandler(
    UserManager<AppUser> userManager,
    ICompanyUserRepository companyUserRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IMapper mapper) : IRequestHandler<CreateUserCommand, IDomainResult<string>>
{
    public async Task<IDomainResult<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isUserExists = await userManager.Users.AnyAsync(u => u.UserName == request.UserName || u.Email == request.Email, cancellationToken);
        if (isUserExists) return DomainResult<string>.Failed("User already exists");

        AppUser user = mapper.Map<AppUser>(request);

        IdentityResult identityResult = await userManager.CreateAsync(user, request.Password);
        if (!identityResult.Succeeded) return DomainResult<string>.Failed(identityResult.Errors.Select(e => e.Description).ToList());

        await CompanyUsersAsync(request, user, cancellationToken);

        cacheService.Remove("users");

        return DomainResult.Success("Kullanıcı başarıyla oluşturuldu");
    }

    private async Task CompanyUsersAsync(CreateUserCommand request, AppUser user, CancellationToken cancellationToken)
    {
        List<CompanyUser> companyUsers = request.CompanyIds.Select(companyId => new CompanyUser
        {
            CompanyId = Guid.Parse(companyId),
            AppUserId = user.Id
        }).ToList();

        await companyUserRepository.AddRangeAsync(companyUsers);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
