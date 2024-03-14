using VersaLog_server.Controllers;
using System.Web.Http.Cors;
using System.Web.Http;

namespace VersaLog_server.Startup;

public static class MapEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}