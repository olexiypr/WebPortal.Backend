using System.Reflection;
using WebPortal.Application.Extensions;
using WebPortal.Application.Mapping;

namespace WebPortal.WebAPI.ServiceExtension;

public class AutoMapperInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(ArticleMapper).Assembly);
    }
}