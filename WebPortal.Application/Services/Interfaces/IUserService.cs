using Microsoft.AspNetCore.Http;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Models;

namespace WebPortal.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserModel> GetUserByIdAsync(Guid id);
    Task<UserModel> GetUserByNickName(string nickName);
    Task<UserModel> UpdateUserData(UpdateUserDataDto userDataDto);
    Task<UserModel> UpdateUserPhoto(IFormFile avatar, string nickName);
    Task<UserModel> AddComplain(UpdateUserComplainDto complainDto);
}