using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services.Cache;

class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
{
    public T Get<T>(string key) => memoryCache.Get<T>(key)!;

    public bool Remove(string key)
    {
        memoryCache.Remove(key);
        return !memoryCache.TryGetValue(key, out _);
    }

    public void Set<T>(string key, T value, TimeSpan? expiry = null) =>
        memoryCache.Set(key, value, TimeSpan.FromHours(1));
}