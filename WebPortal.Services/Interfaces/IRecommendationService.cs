using WebPortal.Application.Models.Article;

namespace WebPortal.Services.Interfaces;

public interface IRecommendationService
{
    public Task<IEnumerable<ArticlePreviewModel>> GetRecommendation();
}