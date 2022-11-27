using WebPortal.Domain;
using WebPortal.Domain.Enums;

namespace WebPortal.Application.Dtos.Article;

public class CreateArticleDto
{
    public Guid AuthorId { get; set; }
    public ArticleStatuses Status { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public string? Text { get; set; }
    public ICollection<TagDto>? Tags { get; set; }
}