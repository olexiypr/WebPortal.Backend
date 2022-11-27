using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.ArticleCategory;
using WebPortal.Application.Models.Article;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class ArticleCategoryService : IArticleCategoryService
{
    private readonly IRepository<ArticleCategory> _articleCategoryRepository;
    private readonly IMapper _mapper;
    public ArticleCategoryService(IRepository<ArticleCategory> articleCategoryRepository, IMapper mapper) =>
        (_articleCategoryRepository, _mapper) = (articleCategoryRepository, mapper);

    public async Task<IEnumerable<ArticleCategoryModel>> GetAllCategories()
    {
        var categories = await _articleCategoryRepository.Query().ToListAsync();
        return _mapper.ProjectTo<ArticleCategoryModel>(categories.AsQueryable());
    }

    public async Task<ArticleCategoryModel> GetArticlesInCategory(Guid categoryId, PaginationDto paginationDto)
    {
        var countArticles = paginationDto.Count;
        var pageNumber = paginationDto.PageNumber;
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
        if (category.Articles!.Count == 0 || category.Articles.Count <= countArticles)
        {
            var categoryModel = _mapper.Map<ArticleCategoryModel>(category);
            categoryModel.Articles = category.Articles.Select(article => _mapper.Map<ArticlePreviewModel>(article));
            return categoryModel;
        }

        if (category.Articles.Count < countArticles * pageNumber)
        {
            throw new ArgumentException(nameof(Article), categoryId.ToString());
        }
        if (category.Articles.Skip(countArticles * pageNumber).Count() < countArticles)
        {
            category.Articles = category.Articles.Skip(countArticles * pageNumber).ToArray();
            return _mapper.Map<ArticleCategoryModel>(category);
        }
        category.Articles = category.Articles.Skip(countArticles * pageNumber).Take(countArticles).ToArray();
        return _mapper.Map<ArticleCategoryModel>(category);
    }

    public async Task<ArticleCategoryModel> CreateArticleCategory(CreateArticleCategoryDto createArticleCategoryDto)
    {
        var category = _mapper.Map<ArticleCategory>(createArticleCategoryDto);
        await _articleCategoryRepository.AddAsync(category);
        await _articleCategoryRepository.SaveChangesAsync();
        return _mapper.Map<ArticleCategoryModel>(category);
    }
}