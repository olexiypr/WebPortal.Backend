using WebPortal.Application.Dtos;
using WebPortal.Application.Models.Article;
using WebPortal.Domain;

namespace WebPortal.Application.Services.Interfaces;

public interface IPaginationService
{
    IEnumerable<Article>
        GetArticlesByPagination(IEnumerable<Article> articles, PaginationDto? paginationDto);
}