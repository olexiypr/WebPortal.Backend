using WebPortal.Application.Dtos.Auth;

namespace Services.Interfaces.Identity;

public interface IIdentityService
{
    public Task<string> GetToken(AuthDto authDto);
}