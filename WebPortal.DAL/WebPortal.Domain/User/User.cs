using WebPortal.Domain.Complaint;
using WebPortal.Domain.Enums;

namespace WebPortal.Domain.User;

public class User : UserAuth
{
    public string NickName { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public ICollection<Article> Articles { get; set; } = new List<Article>();
    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
    public DateTime RegistrationDate { get; set; }
    public Recommendation? Recommendation { get; set; }
    public ICollection<UserComplaint> Complaints { get; set; } = new List<UserComplaint>();
}