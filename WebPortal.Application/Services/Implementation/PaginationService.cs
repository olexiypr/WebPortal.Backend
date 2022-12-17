using AutoMapper;
using WebPortal.Application.Dtos;
using WebPortal.Application.Models.Article;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;

namespace WebPortal.Application.Services.Implementation;

public class PaginationService : IPaginationService
{
    private readonly IMapper _mapper;

    public PaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IEnumerable<ArticlePreviewModel> GetArticlesByPagination(IEnumerable<Article> articles, PaginationDto? paginationDto)
    {
        if (paginationDto is null)
        {
            return _mapper.ProjectTo<ArticlePreviewModel>(articles.AsQueryable());;
        }
        var countArticles = paginationDto.Count;
        var pageNumber = paginationDto.PageNumber;
        if (!articles.Any() || articles.Count() <= countArticles)
        {
            var previewModels = articles.Select(article => _mapper.Map<ArticlePreviewModel>(article));
            return previewModels;
        }
        if (articles.Count() < countArticles * pageNumber)
        {
            throw new ArgumentException(nameof(Article), paginationDto.ToString());
        }
        if (articles.Skip(countArticles * pageNumber).Count() < countArticles)
        {
            articles = articles.Skip(countArticles * pageNumber - 1).ToArray();
            return _mapper.ProjectTo<ArticlePreviewModel>(articles.AsQueryable());
        }
        articles = articles.Skip(countArticles * pageNumber).Take(countArticles).ToArray();
        return _mapper.ProjectTo<ArticlePreviewModel>(articles.AsQueryable());
    }
}