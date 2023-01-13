using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebPortal.Application.Dtos;
using WebPortal.Domain;
using WebPortal.Persistence.Infrastructure;
using WebPortal.Services.Interfaces.Cache;

namespace WebPortal.Services.Implementation.Cache;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IRepository<Article> _articleRepository;
    private readonly IRepository<Commentary> _commentaryRepository;
    public CacheService(IMemoryCache memoryCache, IRepository<Article> articleRepository)
    {
        _memoryCache = memoryCache;
        _articleRepository = articleRepository;
    }

    public async Task<IEnumerable<Article>> GetArticlesFromCache(PaginationDto paginationDto)
    {
        if (!_memoryCache.TryGetValue(CacheKeys.PopularArticlesInCacheKey, out IEnumerable<Article> articles))
        {
            articles = await _articleRepository.Query()
                .Include(article => article.Tags)
                .Take(paginationDto.Count)
                .ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(20));
            _memoryCache.Set(CacheKeys.PopularArticlesInCacheKey, articles, cacheEntryOptions);
        }

        return articles;
    }

    public async Task<IEnumerable<Commentary>> GetCommentariesFromCache(Guid id)
    {
        if (!_memoryCache.TryGetValue(id, out IEnumerable<Commentary> commentaries))
        {
            commentaries = await _commentaryRepository.Query()
                .Include(commentary => commentary.Author)
                .Include(commentary => commentary.Replies)
                .ThenInclude(reply => reply.Replies)
                .Where(commentary => commentary.ArticleId == id)
                .ToListAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(20));
            _memoryCache.Set(id, commentaries, cacheEntryOptions);
        }
        return commentaries;
    }
}