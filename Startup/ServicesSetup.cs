
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace VersaLog_server.Startup;
public static class ServicesSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, string connection)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddDbContext<VersaDbContext>(options =>
            options.UseNpgsql(connection));

        services.AddMvc();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VersaLog API", Version = "v1" });
        });
        
        return services;
    }
}