namespace WebPortal.Application.Models.Article;

public class ArticleCategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<ArticlePreviewModel>? Articles { get; set; }
    public int CountArticles { get; set; }
}