using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.ArticleCategory;
using WebPortal.Application.Models.Article;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace Services.Implementation;

public class ArticleCategoryService : IArticleCategoryService
{
    private readonly IRepository<ArticleCategory> _articleCategoryRepository;
    private readonly IPaginationService _paginationService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private const string AllCategoriesInCacheKey = "categories";
    public ArticleCategoryService(IRepository<ArticleCategory> articleCategoryRepository, IMapper mapper, IPaginationService paginationService, IMemoryCache memoryCache)
    {
        _paginationService = paginationService;
        _memoryCache = memoryCache;
        _articleCategoryRepository = articleCategoryRepository;
        _mapper =  mapper;
    }

    public async Task<IEnumerable<ArticleCategoryModel>> GetAllCategoriesAsync()
    {
        if (_memoryCache.TryGetValue(AllCategoriesInCacheKey, out IEnumerable<ArticleCategory> categories))
            return _mapper.ProjectTo<ArticleCategoryModel>(categories!.AsQueryable());
        categories = await _articleCategoryRepository.Query().ToListAsync();
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(1));
        _memoryCache.Set(AllCategoriesInCacheKey, categories, cacheEntryOptions);
        return _mapper.ProjectTo<ArticleCategoryModel>(categories.AsQueryable());
    }
    
    public async Task<ArticleCategoryModel> GetArticlesInCategory(Guid categoryId, PaginationDto paginationDto)
    {
        var category = await _articleCategoryRepository
            .Query()
            .Include(articleCategory => articleCategory.Articles)
            .ThenInclude(article => article.Tags)
            .Where(articleCategory => 
                articleCategory.Articles.Select(article => article.Status).Contains(ArticleStatuses.Published))
            .FirstOrDefaultAsync(articleCategory => articleCategory.Id == categoryId);
        if (category == null)
        {
            throw new NotFoundException(nameof(ArticleCategory), categoryId);
        }
        var articlePreviews = _mapper.ProjectTo<ArticlePreviewModel>(
            _paginationService.GetArticlesByPagination(category.Articles, paginationDto).AsQueryable());
        var categoryModel = _mapper.Map<ArticleCategoryModel>(category);
        categoryModel.Articles = articlePreviews;
        return categoryModel;
    }

    public async Task<ArticleCategoryModel> CreateArticleCategory(CreateArticleCategoryDto createArticleCategoryDto)
    {
        var category = _mapper.Map<ArticleCategory>(createArticleCategoryDto);
        await _articleCategoryRepository.AddAsync(category);
        await _articleCategoryRepository.SaveChangesAsync();
        return _mapper.Map<ArticleCategoryModel>(category);
    }
}