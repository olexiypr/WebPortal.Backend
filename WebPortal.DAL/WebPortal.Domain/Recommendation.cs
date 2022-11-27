namespace WebPortal.Domain;

public class Recommendation : BaseEntity
{
    public User.User User { get; set; }
    public Guid UserId { get; set; }
    public List<string> FoundWords { get; set; } = new List<string>();
}