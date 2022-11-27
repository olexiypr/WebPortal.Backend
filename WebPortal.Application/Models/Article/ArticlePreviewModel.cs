namespace WebPortal.Application.Models.Article;

public class ArticlePreviewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int CountLikes { get; set; }
    public double Rating { get; set; }
    public DateTime CreationDate { get; set; }
    public IEnumerable<TagModel>? Tags { get; set; }
    public int CountViews { get; set; }
}