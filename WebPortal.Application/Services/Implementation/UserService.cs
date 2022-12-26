using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Exceptions;
using WebPortal.Application.Extensions;
using WebPortal.Application.Models;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IImageService _imageService;
    public UserService(IRepository<User> userRepository,
        IMapper mapper, IHttpContextAccessor contextAccessor, IImageService imageService)
    {
        _contextAccessor = contextAccessor;
        _imageService = imageService;
        (_userRepository, _mapper) =
            (userRepository, mapper);
    }

    public async Task<UserModel> GetCurrentUser()
    {
        var id = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        var user = await _userRepository.Query()
            .Include(user => user.Articles)
            .ThenInclude(article => article.Tags)
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(u => u.Id == id);
        return _mapper.Map<UserModel>(user);
    }
    public async Task<UserModel> GetUserByNickName(string nickName)
    {
        var user = await _userRepository.Query()
            .Include(user => user.Articles
                .Where(article => article.Status == ArticleStatuses.Published))
            .ThenInclude(article => article.Tags)
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(u => u.NickName == nickName);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), nickName);
        }
        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
    }
    
    public async Task<UserModel> UpdateUserData(UpdateUserDataDto userDataDto)
    {
        var userId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        var user = await _userRepository.Query()
            .FirstOrDefaultAsync(user => user.Id == userId);
        if (user == null)
        {
            throw new UserAccessDeniedExceptions(nameof(User));
        }

        if (userDataDto.ChangedNickName != null && 
            await _userRepository.Query().AnyAsync(user => user.NickName == userDataDto.ChangedNickName))
        {
            throw new ArgumentException("User with same nick name already registered!");
        }
        user.NickName = userDataDto.ChangedNickName ?? user.NickName;
        user.Name = userDataDto.Name ?? user.Name;
        user.Description = userDataDto.Description ?? user.Description;
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> UpdateUserPhoto(IFormFile avatar, string nickName)
    {
        var user = await _userRepository.Query().FirstOrDefaultAsync(user => user.NickName == nickName);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), nickName);
        }

        await _imageService.SetAvatar(user, avatar);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserModel>(user);
    }

    public Task<UserModel> AddComplain(UpdateUserComplainDto complainDto)
    {
        throw new NotImplementedException();
    }
}