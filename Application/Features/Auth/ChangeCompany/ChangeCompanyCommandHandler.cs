using Domain.Entities;
using Domain.Interfaces;
using DomainResults.Common;
using Infrastructure.Configurations;
using Infrastructure.Services.Jwt;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Features.Auth.ChangeCompany;

class ChangeCompanyCommandHandler(
    ICompanyUserRepository companyUserRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<ChangeCompanyCommand, IDomainResult<LoginCommandResponse>>
{
    public async Task<IDomainResult<LoginCommandResponse>> Handle(ChangeCompanyCommand request, CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext is null) return DomainResult.NotFound<LoginCommandResponse>("Bu işlemi yapmaya yetkiniz yok");

        String? userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(userId)) return DomainResult.NotFound<LoginCommandResponse>("Bu işlemi yapmaya yetkiniz yok");

        AppUser? user = await userManager.FindByIdAsync(userId);
        if (user is null) return DomainResult.NotFound<LoginCommandResponse>("Kullanıcı bulunamadı");

        List<CompanyUser> companyUsers = await companyUserRepository
            .Where(c => c.AppUserId == user.Id)
            .Include(c => c.Company)
            .ToListAsync(cancellationToken);

        List<Company> companies = companyUsers.Select(c => new Company
        {
            Id = c.CompanyId,
            Name = c.Company!.Name,
            TaxDepartment = c.Company.TaxDepartment,
            TaxNumber = c.Company.TaxNumber,
            FullAddress = c.Company.FullAddress
        }).ToList();

        LoginCommandResponse response = await jwtTokenGenerator.CreateToken(user, request.CompanyId, companies);
        return DomainResult.Success(response);
    }
}