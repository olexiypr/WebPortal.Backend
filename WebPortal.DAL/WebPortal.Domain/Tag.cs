namespace WebPortal.Domain;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public ICollection<User.User> Finders { get; set; } = new List<User.User>();
    public ICollection<Article> Articles { get; set; } = new List<Article>();
}