using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Interfaces.Identity;
using WebPortal.Application.Auth;
using WebPortal.Application.Dtos.Auth;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Models;
using WebPortal.Domain.User;
using WebPortal.Persistence.Infrastructure;

namespace Services.Implementation.Identity;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IRepository<User> _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IImageService _imageService;
    private readonly IHashService _hashService;
    public AuthService(IMapper mapper, IRepository<User> userRepository, IIdentityService identityService, IImageService imageService, IHashService hashService)
    {
        _imageService = imageService;
        _hashService = hashService;
        (_mapper, _userRepository, _identityService) = (mapper, userRepository, identityService);
    }

    public async Task<(string, UserModel)> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        if (await IsRegisterUser(registerUserDto.Email))
        {
            throw new UserAccessDeniedExceptions("User already registered!");
        }
        var user = _mapper.Map<User>(registerUserDto);
        user.RegistrationDate = DateTime.Now;
        user.Role = AuthOptions.UserRole;
        user.Password = _hashService.HashPassword(user.Password);
        await _userRepository.AddAsync(user);
        
        if (registerUserDto.Avatar != null)
        {
            await _imageService.SetAvatar(user, registerUserDto.Avatar);
        }
        await _userRepository.SaveChangesAsync();
        var authDto = _mapper.Map<AuthDto>(user);
        var token = await _identityService.GetToken(authDto);
        return (token, _mapper.Map<UserModel>(user));
    }
    
    private async Task<bool> IsRegisterUser(string email)
    {
        var user = await _userRepository.Query()
            .FirstOrDefaultAsync(user => user.Email == email);
        return user != null;
    }
    public async Task<(string, UserModel)> LoginUserAsync(AuthDto authDto)
    {
        var token = await _identityService.GetToken(authDto);
        var user = await _userRepository.Query().FirstOrDefaultAsync(user => user.Email == authDto.Email);
        var userModel = _mapper.Map<UserModel>(user);
        return (token, userModel);
    }
}