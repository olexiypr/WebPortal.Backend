using AutoMapper;
using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Models.Article;
using WebPortal.Domain;

namespace WebPortal.Application.Mapping;

public class ArticleMapper : Profile
{
    public ArticleMapper()
    {
        CreateMap<Article, ArticleModel>().ForMember(model => model.AuthorNickName,
            opt => opt.MapFrom(article => article.Author.NickName))
            .ForMember(model => model.Tags,
                opt => opt.MapFrom(article => article.Tags))
            .ForMember(model => model.CategoryName,
                opt => opt.MapFrom(article => article.Category!.Name));
        
        CreateMap<CreateArticleDto, Article>()
            .ForMember(article => article.CreationDate, 
                opt => opt.MapFrom(dto => DateTime.Now))
            .ForMember(article => article.Tags,
                opt => opt.Ignore());

        CreateMap<UpdateArticleDataDto, Article>();
        CreateMap<Article, ArticlePreviewModel>().ReverseMap();
    }
}