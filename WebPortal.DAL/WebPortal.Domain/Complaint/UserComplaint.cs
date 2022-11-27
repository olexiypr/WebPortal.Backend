using WebPortal.Domain.Enums;

namespace WebPortal.Domain.Complaint;

public class UserComplaint : BaseComplaint
{
    public User.User User { get; set; }
    public UserComplainCategories Category { get; set; }
}