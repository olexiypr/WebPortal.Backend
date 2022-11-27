using Microsoft.AspNetCore.Http;

namespace WebPortal.Application.Dtos.User;

public class UpdateUserPhotoDto
{
    public string? NickName { get; set; }
    public IFormFile Image { get; set; }
}