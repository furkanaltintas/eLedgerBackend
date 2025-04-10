using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Services.Cache;

class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connection) => _database = connection.GetDatabase();

    public T Get<T>(string key)
    {
        RedisValue? value = _database.StringGet(key);
        if (value.HasValue)
            return JsonConvert.DeserializeObject<T>(value!)!;
        else
            return default!;
    }

    public bool Remove(string key)
    {
        return _database.KeyDelete(key);
    }

    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        String serializedValue = JsonConvert.SerializeObject(value);
        _database.StringSet(key, serializedValue, expiry);
    }
}