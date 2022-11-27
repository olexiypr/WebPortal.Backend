using WebPortal.Application.Models.Commentary;

namespace WebPortal.Application.Models.Article;

public class ArticleModel
{
    public Guid Id { get; set; }
    public string AuthorNickName { get; set; }
    public Guid AuthorId { get; set; }
    public string Name { get; set; }
    public string CategoryName { get; set; }
    public Guid CategoryId { get; set; }
    public string? Text { get; set; }
    public int CountLikes { get; set; }
    public int CountDislikes { get; set; }
    public int CountAppraisers { get; set; }
    public double Rating { get; set; }
    public DateTime CreationDate { get; set; }
    public IEnumerable<TagModel> Tags { get; set; }
    public IEnumerable<CommentaryModel>? Commentaries { get; set; }
    public IEnumerable<ArticlePreviewModel> Recommendation { get; set; }
    public int CountViews { get; set; }
}