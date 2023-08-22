using TinyCRM.Application.Cache.Interface;
using TinyCRM.Application.Models;

namespace TinyCRM.Application.Cache;

public class PermissionCacheManager : IPermissionCacheManager
{
    private readonly ICacheService _cacheService;
    private readonly JwtSettings _jwtSettings;

    public PermissionCacheManager(ICacheService cacheService, JwtSettings jwtSettings)
    {
        _cacheService = cacheService;
        _jwtSettings = jwtSettings;
    }

    public Task<IEnumerable<string>?> GetForRoleAsync(string role)
    {
        return _cacheService.GetRecordAsync<IEnumerable<string>?>
            (KeyGenerator.Generate(CacheTarget.PermissionRole, role));
    }


    public Task SetForRoleAsync(string role, IEnumerable<string> permissions)
    {
        return _cacheService.SetRecordAsync(KeyGenerator.Generate(CacheTarget.PermissionRole, role),
            permissions, TimeSpan.FromMinutes(_jwtSettings.ExpiryInMinutes));
    }

    public Task ClearForRoleAsync(string role)
    {
        return _cacheService.RemoveRecordAsync(KeyGenerator.Generate(CacheTarget.PermissionRole, role));
    }
}