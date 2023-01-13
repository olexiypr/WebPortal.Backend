using WebPortal.Application.Extensions;
using WebPortal.Domain;
using WebPortal.Domain.User;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.WebAPI.ServiceExtension;

public class DbInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        
        
        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IRepository<UserAuth>, Repository<UserAuth>>();
        services.AddScoped<IRepository<Article>, Repository<Article>>();
        services.AddScoped<IRepository<ArticleCategory>, Repository<ArticleCategory>>();
        services.AddScoped<IRepository<Tag>, Repository<Tag>>();
        services.AddScoped<IRepository<Recommendation>, Repository<Recommendation>>();
        services.AddScoped<IRepository<Commentary>, Repository<Commentary>>();
       
    }
}