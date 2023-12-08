using Aquantica.DAL;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.API.Extensions;

public static class MigrationManager
{
    public static async Task MigrateDatabaseAsync(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();

        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(MigrationManager));

        await using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            logger.LogInformation("MigrationManager: Trying to migrate database");
            await appContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "MigrationManager: Database migration failed.");
            throw;
        }

        logger.LogInformation("MigrationManager: The database migration was successful");
    }
}