
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace VersaLog_server.Startup;
public static class ServicesSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, string connection)
    {
        services.AddEndpointsApiExplorer();

        services.AddDbContext<VersaDbContext>(options =>
            options.UseNpgsql(connection));

        services.AddMvc();
        
        services.AddSwaggerGen();
        return services;
    }
}