namespace WebPortal.WebAPI.ServiceExtension;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}