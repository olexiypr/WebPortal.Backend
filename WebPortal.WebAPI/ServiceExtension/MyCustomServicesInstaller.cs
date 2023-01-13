using WebPortal.Services.Implementation;
using WebPortal.Services.Implementation.Identity;
using WebPortal.Services.Interfaces;
using WebPortal.Services.Interfaces.Identity;

namespace WebPortal.WebAPI.ServiceExtension;

public class MyCustomServicesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPaginationService, PaginationService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentaryService, CommentaryService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
        services.AddScoped<ISearchService, SearchService>();
    }
}