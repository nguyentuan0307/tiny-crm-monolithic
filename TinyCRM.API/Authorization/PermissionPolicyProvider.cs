using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TinyCRM.API.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private const string PermissionPolicyPrefix = "TinyCrm.Permissions";

    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => Task.FromResult<AuthorizationPolicy?>(null);

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(PermissionPolicyPrefix, StringComparison.OrdinalIgnoreCase))
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new PermissionRequirement(policyName));
        return Task.FromResult(policy.Build())!;
    }
}