namespace VersaLog.Startup;

public static class MapEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}