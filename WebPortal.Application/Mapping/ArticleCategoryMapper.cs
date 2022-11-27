using AutoMapper;
using WebPortal.Application.Dtos.ArticleCategory;
using WebPortal.Application.Models.Article;
using WebPortal.Domain;

namespace WebPortal.Application.Mapping;

public class ArticleCategoryMapper : Profile
{
    public ArticleCategoryMapper()
    {
        CreateMap<ArticleCategory, ArticleCategoryModel>()
            .ForMember(model => model.CountArticles, 
                opt => opt.MapFrom(category => category.Articles!.Count));

        
        CreateMap<CreateArticleCategoryDto, ArticleCategory>().ReverseMap();
    }
}