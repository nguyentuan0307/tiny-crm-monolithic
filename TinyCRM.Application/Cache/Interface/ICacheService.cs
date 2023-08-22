namespace TinyCRM.Application.Cache.Interface;

public interface ICacheService
{
    Task<bool> SetRecordAsync<T>(string key, T data, TimeSpan expireTime);

    Task<T?> GetRecordAsync<T>(string key);

    Task<bool> RemoveRecordAsync(string key);

    Task ClearAsync();
}