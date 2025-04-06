using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginCommandResponse> CreateToken(AppUser user, Guid companyId, List<Company> companies)
    {
        DateTime expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryInDays);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey ?? string.Empty));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken securityToken = new(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: expires,
            claims: GetClaims(user, companyId, companies),
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler handler = new();
        String token = handler.WriteToken(securityToken);

        String refreshToken = Guid.NewGuid().ToString();
        DateTime refreshTokenExpires = expires.AddHours(1);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = refreshTokenExpires;

        await _userManager.UpdateAsync(user);
        return new(token, refreshToken, refreshTokenExpires);
    }

    private IEnumerable<Claim> GetClaims(AppUser user, Guid companyId, List<Company> companies)
    {
        List<Claim> claims = new()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString() ?? string.Empty),
            new(ClaimTypes.Name, user.FullName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("UserName", user.UserName ?? string.Empty),
            new("IsAdmin", user.IsAdmin.ToString()),
            new("Companies", JsonSerializer.Serialize(companies)),
            new("CompanyId", companyId.ToString() ?? string.Empty),
        };

        // Kullanıcının rollerini ekle
        //IList<string>? roles = await _userManager.GetRolesAsync(user);
        //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}