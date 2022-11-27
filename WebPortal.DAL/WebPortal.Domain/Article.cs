using WebPortal.Domain.Complaint;
using WebPortal.Domain.Enums;

namespace WebPortal.Domain;

public class Article : BaseEntity
{
    public User.User Author { get; set; }
    public Guid AuthorId { get; set; }
    public string Name { get; set; }
    public List<string> KeyWords { get; set; } = new List<string>();
    public ArticleCategory? Category { get; set; }
    public Guid CategoryId { get; set; }
    public string Text { get; set; } = String.Empty;
    public int CountLikes { get; set; }
    public int CountDislikes { get; set; }
    public int CountViewsPerDay { get; set; }
    public int CountViewsPerWeek { get; set; }
    public int CountViewsPerMonth { get; set; }
    public int CountViews { get; set; }
    public double Rating { get; set; }
    public int CountAppraisers { get; set; }
    public DateTime CreationDate { get; set; }
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
    public ArticleStatuses Status { get; set; }
    public ICollection<ArticleComplaint> Complaints { get; set; } = new List<ArticleComplaint>();
    
}