using WebPortal.Domain.Enums;

namespace WebPortal.Application.Models.Article;

public class UserArticlePreviewModel : ArticlePreviewModel
{
    public ArticleStatuses Status { get; set; }
    public int CountViewsPerDay { get; set; }
    public int CountViewsPeyWeek { get; set; }
    public int CountViewsPerMonth { get; set; }
}