namespace WebPortal.Domain;

public class ArticleCategory : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}