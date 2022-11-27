using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Dtos.Auth;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Models;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class RegisterService : IRegisterService
{
    private readonly IMapper _mapper;
    private readonly IRepository<User> _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IWebHostEnvironment _environment;
    public RegisterService(IMapper mapper, IRepository<User> userRepository, IIdentityService identityService, IWebHostEnvironment environment)
    {
        _environment = environment;
        (_mapper, _userRepository, _identityService) = (mapper, userRepository, identityService);
    }

    public async Task<(string, UserModel)> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var user = _mapper.Map<User>(registerUserDto);
        user.RegistrationDate = DateTime.Now;
        user.Role = "user";
        if (registerUserDto.Avatar != null)
        {
            SetAvatar(user, registerUserDto.Avatar);
        }
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        var authDto = _mapper.Map<AuthDto>(user);
        var token = await _identityService.GetToken(authDto);
        return (token, _mapper.Map<UserModel>(user));
    }

    private async void SetAvatar(User user, IFormFile avatar)
    {
        var path = $"/Avatars/{user.Id}{Path.GetFileName(avatar.FileName)}";
        try
        {
            await using var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create);
            await avatar.CopyToAsync(fileStream);
            user.Avatar = path;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<(string, UserModel)> LoginUserAsync(AuthDto authDto)
    {
        var token = await _identityService.GetToken(authDto);
        var user = await _userRepository.Query().FirstOrDefaultAsync(user => user.Email == authDto.Email);
        var userModel = _mapper.Map<UserModel>(user);
        return (token, userModel);
    }
}