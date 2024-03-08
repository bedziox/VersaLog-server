
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace VersaLog_server.Startup;
public static class ServicesSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddDbContext<VersaDbContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));
        
        services.AddSwaggerGen();
        return services;
    }
}