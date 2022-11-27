using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WebPortal.Application.Mapping;

namespace WebPortal.Application.Extensions;

public static class AutoMapperExtension
{
    public static void AddAutoMapperr(this IServiceCollection services)
    {
        
        
        /*var mapperConfig = new MapperConfiguration(config =>
        {
            
            
            config.AddProfile(new UserMapper());
            config.AddProfile(new ArticleMapper());
            config.AddProfile(new ArticleCategoryMapper());
            config.AddProfile(new TagMapper());
            config.AddProfile(new SearchMapper());
            config.AddProfile(new CommentaryMapper());
        });
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);*/
    }
}