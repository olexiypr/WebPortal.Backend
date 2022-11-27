using WebPortal.Domain.Complaint;
using WebPortal.Domain.Enums;

namespace WebPortal.Application.Dtos.Complain;

public class ArticleComplainDto
{
    public ArticleComplainCategories Category { get; set; }
    public string? Text { get; set; }
    public Guid ArticleId { get; set; }
}