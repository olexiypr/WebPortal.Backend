using Microsoft.AspNetCore.Http;
using WebPortal.Domain.User;

namespace WebPortal.Services.Interfaces;

public interface IImageService
{
    /*public Task<UserModel> UpdateImageAsync(string nickName, IFormFile file);*/
    public Task<FileStream?> GetImageByUserId(Guid id);
    public Task SetAvatar(User user, IFormFile avatar);
}