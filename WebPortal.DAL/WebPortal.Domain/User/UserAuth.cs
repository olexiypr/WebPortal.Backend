namespace WebPortal.Domain.User;

public class UserAuth : BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}