using WebPortal.Application.Dtos.Auth;

namespace WebPortal.Application.Services.Interfaces;

public interface IIdentityService
{
    public Task<string> GetToken(AuthDto authDto);
}