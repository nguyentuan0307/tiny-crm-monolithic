namespace TinyCRM.Application.Cache.Interface;

public interface IPermissionCacheManager
{
    Task<IEnumerable<string>?> GetForRoleAsync(string role);

    Task SetForRoleAsync(string role, IEnumerable<string> permissions);

    Task ClearForRoleAsync(string role);
}