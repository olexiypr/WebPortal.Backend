using System.Globalization;

namespace WebPortal.Application.Dtos.Commentary;

public class AddCommentaryDto
{
    public Guid? ReplayToId { get; set; }
    public Guid ArticleId { get; set; }
    public string Text { get; set; } = string.Empty;
}