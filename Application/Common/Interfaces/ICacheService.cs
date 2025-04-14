﻿namespace Application.Common.Interfaces;

public interface ICacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? expiry = null);
    bool Remove(string key);
}