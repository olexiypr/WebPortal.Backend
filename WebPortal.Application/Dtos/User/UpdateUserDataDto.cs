namespace WebPortal.Application.Dtos.User;

public class UpdateUserDataDto
{
    public string NickName { get; set; }
    public string? ChangedNickName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}