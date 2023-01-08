using Serilog;
using Serilog.Events;

namespace WebPortal.WebAPI.ServiceExtension;

public class SerilogInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("webPortalLog-.log", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}