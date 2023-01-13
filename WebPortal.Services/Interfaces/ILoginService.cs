using WebPortal.Application.Models;

namespace WebPortal.Services.Interfaces;

public interface ILoginService
{
    public Task<UserModel> LoginAsync();
    public Task<UserModel> LogoutAsync();
}