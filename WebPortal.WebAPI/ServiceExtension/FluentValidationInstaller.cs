using FluentValidation;
using WebPortal.Application.Mapping;

namespace WebPortal.WebAPI.ServiceExtension;

public class FluentValidationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(UserMapper).Assembly);
    }
}