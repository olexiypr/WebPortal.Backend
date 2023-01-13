using AutoMapper;
using WebPortal.Application.Dtos;
using WebPortal.Domain;
using WebPortal.Services.Interfaces;

namespace WebPortal.Services.Implementation;

public class PaginationService : IPaginationService
{
    private readonly IMapper _mapper;

    public PaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IEnumerable<Article> GetArticlesByPagination(IEnumerable<Article> articles, PaginationDto? paginationDto)
    {
        if (paginationDto is null || (paginationDto.Count == 0 && paginationDto.PageNumber == 0))
        {
            return articles;
        }
        var countArticles = paginationDto.Count;
        var pageNumber = paginationDto.PageNumber;
        if (!articles.Any() || articles.Count() <= countArticles)
        {
            return articles;
        }
        if (articles.Count() < countArticles * pageNumber)
        {
            throw new ArgumentException(nameof(Article), paginationDto.ToString());
        }
        if (articles.Skip(countArticles * pageNumber).Count() < countArticles)
        {
            return articles.Skip(countArticles * pageNumber - 1).ToArray();
        }
        return articles.Skip(countArticles * pageNumber).Take(countArticles).ToArray();
    }
}