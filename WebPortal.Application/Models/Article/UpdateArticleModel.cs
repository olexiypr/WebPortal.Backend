namespace WebPortal.Application.Models.Article;

public class UpdateArticleModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ArticleCategoryModel Category { get; set; }
    public string? Text { get; set; }
    public IEnumerable<TagModel> Tags { get; set; }
}