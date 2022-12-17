using WebPortal.Application.Services;
using WebPortal.Application.Services.Implementation;
using WebPortal.Application.Services.Interfaces;

namespace WebPortal.WebAPI.ServiceExtension;

public class MyCustomServicesInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRecommendationService, RecommendationService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IPaginationService, PaginationService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentaryService, CommentaryService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
        services.AddScoped<ISearchService, SearchService>();
    }
}