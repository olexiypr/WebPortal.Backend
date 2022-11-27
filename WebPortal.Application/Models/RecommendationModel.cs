using WebPortal.Application.Models.Article;

namespace WebPortal.Application.Models;

public class RecommendationModel
{
    public IEnumerable<ArticlePreviewModel> Articles { get; set; }
}