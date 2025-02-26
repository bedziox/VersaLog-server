using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using VersaLog;
using VersaLog.Utils;

public static class ServicesSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, DbOptions dbOptions)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        Console.WriteLine($"{dbOptions.DatabaseName}, {dbOptions.Username}, {dbOptions.Password}");
        services.AddDbContext<VersaDbContext>(options =>
            options.UseNpgsql(dbOptions.CreateConnectionString()));

        services.AddMvc();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VersaLog API", Version = "v1" });
        });

        return services;
    }
}