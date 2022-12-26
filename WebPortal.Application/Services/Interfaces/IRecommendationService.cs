using WebPortal.Application.Models;
using WebPortal.Application.Models.Article;
using WebPortal.Domain;

namespace WebPortal.Application.Services.Interfaces;

public interface IRecommendationService
{
    public Task<IEnumerable<ArticlePreviewModel>> GetRecommendation();
}