using FluentValidation;
using WebPortal.Application.Mapping;
using WebPortal.Application.Validation;

namespace WebPortal.WebAPI.ServiceExtension;

public class FluentValidationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(RegisterUserDtoValidator).Assembly);
    }
}