using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.ArticleCategory;
using WebPortal.Application.Models.Article;

namespace WebPortal.Application.Services.Interfaces;

public interface IArticleCategoryService
{
    public Task<IEnumerable<ArticleCategoryModel>> GetAllCategoriesAsync();
    public Task<ArticleCategoryModel> GetArticlesInCategory(Guid categoryId, PaginationDto paginationDto);
    public Task<ArticleCategoryModel> CreateArticleCategory(CreateArticleCategoryDto createArticleCategoryDto);
}