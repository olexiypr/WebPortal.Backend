using Microsoft.AspNetCore.Http;
using WebPortal.Application.Dtos.Auth;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Models;

namespace WebPortal.Application.Services.Interfaces;

public interface IAuthService
{
    public Task<(string, UserModel)> RegisterUserAsync(RegisterUserDto registerUserDto);
    public Task<(string, UserModel)> LoginUserAsync(AuthDto authDto);
}