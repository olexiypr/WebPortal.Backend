using Microsoft.AspNetCore.Http;
using WebPortal.Domain;

namespace WebPortal.Application.Dtos.User;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string NickName { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Avatar { get; set; }
}