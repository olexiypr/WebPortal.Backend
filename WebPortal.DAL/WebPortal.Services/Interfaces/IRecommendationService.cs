using WebPortal.Application.Models.Article;

namespace Services.Interfaces;

public interface IRecommendationService
{
    public Task<IEnumerable<ArticlePreviewModel>> GetRecommendation();
}