using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPortal.Persistence.Context;

namespace WebPortal.Application.Extensions;

public static class DbContextExtension
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = ConnectionStringConfig.CsLocalhost;
        services.AddDbContext<WebPortalDbContext>(options => options
            .UseNpgsql(connectionString).EnableSensitiveDataLogging());
    }
}