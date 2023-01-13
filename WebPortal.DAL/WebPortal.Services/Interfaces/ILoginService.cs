using WebPortal.Application.Models;

namespace Services.Interfaces;

public interface ILoginService
{
    public Task<UserModel> LoginAsync();
    public Task<UserModel> LogoutAsync();
}