using Application.Common.Interfaces;
using Application.Features.Auth.Login;
using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.LoginByCompany;

class LoginByCompanyCommandHandler(
    UserManager<AppUser> userManager,
    ICompanyUserRepository companyUserRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginByCompanyCommand, IDomainResult<LoginCommandResponse>>
{
    public async Task<IDomainResult<LoginCommandResponse>> Handle(LoginByCompanyCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.UserId);
        if (user is null) return DomainResult<LoginCommandResponse>.NotFound("User not found");

        List<Company> companies = await companyUserRepository
            .Where(c => c.AppUserId == Guid.Parse(request.UserId))
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

        LoginCommandResponse loginResponse = await jwtTokenGenerator.CreateToken(user, Guid.Parse(request.CompanyId), companies);
        return DomainResult.Success(loginResponse);
    }
}
