using Domain.Entities;
using Infrastructure.Configurations;

namespace Infrastructure.Services;

public interface IJwtTokenGenerator
{
    Task<LoginCommandResponse> CreateToken(AppUser user,Guid companyId, List<Company> companies);
}
