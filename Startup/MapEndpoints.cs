namespace VersaLog_server.Startup;

public static class MapEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        // Test endpoints
        app.MapGet("/User", () => "Hello User");
        app.MapGet("/User/{name}", (string name) => $"Hello {name}");
        return app;
    }
}