using WebPortal.Application.Dtos;
using WebPortal.Domain;

namespace WebPortal.Services.Interfaces;

public interface IPaginationService
{
    IEnumerable<Article>
        GetArticlesByPagination(IEnumerable<Article> articles, PaginationDto? paginationDto);
}