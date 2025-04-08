using Domain.Entities;
using Infrastructure.Configurations;

namespace Infrastructure.Services.Jwt;

public interface IJwtTokenGenerator
{
    Task<LoginCommandResponse> CreateToken(AppUser user, Guid companyId, List<Company> companies);
}
