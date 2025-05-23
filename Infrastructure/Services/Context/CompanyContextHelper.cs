﻿using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Context;

internal class CompanyContextHelper(
    IHttpContextAccessor httpContextAccessor,
    ICacheService cacheService) : ICompanyContextHelper
{
    public T GetCompanyFromContext<T>(string name)
    {
        string cacheKey = $"Company_{GetCompanyId()}_{name}";
        T? result = cacheService.Get<T>(cacheKey);
        if (result is null) return default!;
        return result;
    }

    public void RemoveCompanyFromContext(string name)
    {
        cacheService.Remove($"Company_{GetCompanyId()}_{name}");
    }

    public void RemoveRangeCompanyFromContext(string[] names)
    {
        foreach (string name in names) cacheService.Remove($"Company_{GetCompanyId()}_{name}");
    }

    public void SetCompanyInContext<T>(string name, T value)
    {
        cacheService.Set($"Company_{GetCompanyId()}_{name}", value, TimeSpan.FromHours(1));
    }

    private string? GetCompanyId()
    {
        // HttpContext üzerinden CompanyId'yi alıyoruz
        string? companyId = httpContextAccessor.HttpContext?.User.FindFirst("CompanyId")?.Value;
        if (string.IsNullOrEmpty(companyId)) throw new Exception("CompanyId bulunamadı");
        return companyId;
    }
}
