using WebPortal.Domain.Enums;

namespace WebPortal.Domain.Complaint;

public class ArticleComplaint : BaseComplaint
{
    public ArticleComplainCategories Category { get; set; }
    public Article Article { get; set; }
}