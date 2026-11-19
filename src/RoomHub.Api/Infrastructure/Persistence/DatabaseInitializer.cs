using Microsoft.EntityFrameworkCore;

namespace RoomHub.Api.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    // Creates the schema if it is missing. Never EnsureDeleted: the data must survive a restart.
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RoomHubDbContext>();

        if (db.Database.GetMigrations().Any())
            await db.Database.MigrateAsync();
        else
            await db.Database.EnsureCreatedAsync();
    }
}
