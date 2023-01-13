using WebPortal.Application.Dtos;
using WebPortal.Domain;

namespace Services.Interfaces.Cache;

public interface ICacheService
{
    Task<IEnumerable<Article>> GetArticlesFromCache(PaginationDto paginationDto);
    Task<IEnumerable<Commentary>> GetCommentariesFromCache(Guid id);
}