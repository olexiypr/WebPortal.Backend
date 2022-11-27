using WebPortal.Application.Dtos.User;
using WebPortal.Application.Models;

namespace WebPortal.Application.Services.Interfaces;

public interface ILoginService
{
    public Task<UserModel> LoginAsync();
    public Task<UserModel> LogoutAsync();
}