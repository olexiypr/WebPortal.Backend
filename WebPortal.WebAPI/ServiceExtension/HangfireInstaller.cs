using Hangfire;
using Hangfire.MemoryStorage;
using WebPortal.Persistence.Context;

namespace WebPortal.WebAPI.ServiceExtension;

public class HangfireInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config.UseMemoryStorage());
    }
}