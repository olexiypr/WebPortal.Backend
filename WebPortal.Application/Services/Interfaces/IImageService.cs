using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Models;
using WebPortal.Domain.User;

namespace WebPortal.Application.Services.Interfaces;

public interface IImageService
{
    /*public Task<UserModel> UpdateImageAsync(string nickName, IFormFile file);*/
    public Task<FileStream?> GetImageByUserId(Guid id);
    public Task SetAvatar(User user, IFormFile avatar);
}