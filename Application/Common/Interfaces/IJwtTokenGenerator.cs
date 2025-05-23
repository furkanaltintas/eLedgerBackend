﻿using Application.Features.Auth.Login;
using Domain.Entities.Partners;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    Task<LoginCommandResponse> CreateToken(AppUser user, Guid companyId, List<Company> companies);
}
