namespace WebPortal.Application.Dtos.Article;

public class UpdateArticleAnalyticsDto
{
    public Guid Id { get; set; }
    public bool IsLike { get; set; }
    public double? Rating { get; set; }
}