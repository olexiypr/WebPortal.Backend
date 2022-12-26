using FluentValidation;
using FluentValidation.AspNetCore;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Mapping;
using WebPortal.Application.Validation;

namespace WebPortal.WebAPI.ServiceExtension;

public class FluentValidationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
    }
}