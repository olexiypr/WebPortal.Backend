using WebPortal.Application.Models.Article;

namespace WebPortal.Application.Models;

public class SearchModel
{
    public IEnumerable<ArticlePreviewModel>? Articles { get; set; }
}