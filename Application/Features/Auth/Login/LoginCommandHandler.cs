using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Login;

class LoginCommandHandler(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ICompanyUserRepository companyUserRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, IDomainResult<LoginCommandResponse>>
{
    public async Task<IDomainResult<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) return DomainResult<LoginCommandResponse>.NotFound("User not found");

        SignInResult result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (!result.Succeeded) return DomainResult<LoginCommandResponse>.NotFound("Invalid credentials");

        if (result.IsNotAllowed) return DomainResult<LoginCommandResponse>.NotFound("User is not allowed to sign in");

        if (result.IsLockedOut)
        {
            if (user.LockoutEnd.HasValue)
            {
                TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
                if (timeSpan is not null) return DomainResult<LoginCommandResponse>.NotFound($"User is locked out for {timeSpan.Value.TotalMinutes} minutes");
                else return DomainResult<LoginCommandResponse>.NotFound("User is locked out");
            }
            else
            {
                return DomainResult<LoginCommandResponse>.NotFound("User is locked out");
            }
        }

        List<Company> companies = await companyUserRepository
            .Where(c => c.AppUserId == user.Id)
            .Include(c => c.Company)
            .Select(c => new Company
            {
                Id = c.CompanyId,
                Name = c.Company!.Name,
                TaxDepartment = c.Company.TaxDepartment,
                TaxNumber = c.Company.TaxNumber,
                FullAddress = c.Company.FullAddress
            })
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);


        LoginCommandResponse loginResponse;
        if (companies.Count == 1) loginResponse = await jwtTokenGenerator.CreateToken(user, companies.First().Id, companies);
        else loginResponse = await jwtTokenGenerator.CreateToken(user, Guid.Empty, companies);

        return DomainResult.Success(loginResponse);
    }
}
