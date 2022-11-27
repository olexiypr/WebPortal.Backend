using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPortal.Persistence.Context;

namespace WebPortal.Application.Extensions;

public static class DbContextExtension
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        /*var connectionString = "Host=db;Username=aloshaprokopenko5;Password=787898;Database=WebPortalDb";*/ //for docker
        services.AddDbContext<WebPortalDbContext>(options => options
            .UseNpgsql(connectionString));
    }
}