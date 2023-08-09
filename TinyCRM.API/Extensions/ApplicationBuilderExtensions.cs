using TinyCRM.Infrastructure.SeedData;

namespace TinyCRM.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seedData = scope.ServiceProvider.GetRequiredService<DataContributor>();
        var seedPermissions = scope.ServiceProvider.GetRequiredService<PermissionContributor>();
        await seedData.SeedAsync();
        await seedPermissions.SeedPermissionsAsync();
    }
}