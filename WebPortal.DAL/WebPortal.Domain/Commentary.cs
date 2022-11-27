namespace WebPortal.Domain;

public class Commentary : BaseEntity
{
    public User.User Author { get; set; }
    public Guid AuthorId { get; set; }
    public Article Article { get; set; }
    public Guid ArticleId { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
    public int CountLikes { get; set; }
    public int CountDislikes { get; set; }
    public Commentary? Parent { get; set; }
    public ICollection<Commentary> Replies { get; set; } = new List<Commentary>();
}