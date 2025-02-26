using Microsoft.EntityFrameworkCore;

namespace VersaLog.Server.Startup;
public static class MigrationSetup
{
    public static void SetupMigration(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<VersaDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
