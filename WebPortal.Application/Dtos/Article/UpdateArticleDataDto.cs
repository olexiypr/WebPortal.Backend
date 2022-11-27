using WebPortal.Domain;

namespace WebPortal.Application.Dtos.Article;

public class UpdateArticleDataDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Text { get; set; }
    public ICollection<TagDto>? Tags { get; set; }
}