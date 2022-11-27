using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Models.Article;


namespace WebPortal.Application.Services.Interfaces;

public interface IArticleService
{
    public Task<ArticleModel> GetArticleByIdAsync(Guid id);
    public Task<IEnumerable<ArticlePreviewModel>> GetPopularArticles(string period);
    public Task<ArticleModel> CreateArticleAsync(CreateArticleDto articleDto);
    public Task<ArticleModel> UpdateArticleDataAsync(UpdateArticleDataDto updateArticleDataDto);
    public Task<(int, double)> UpdateArticleAnalyticsAsync(UpdateArticleAnalyticsDto updateArticleAnalyticsDto);
    public Task<bool> DeleteArticle(Guid id);
}