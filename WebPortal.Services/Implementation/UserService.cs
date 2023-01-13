using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Extensions;
using WebPortal.Application.Models;
using WebPortal.Domain.Enums;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;
using WebPortal.Services.Interfaces;

namespace WebPortal.Services.Implementation;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IImageService _imageService;
    private readonly IMemoryCache _memoryCache;
    public UserService(IRepository<User> userRepository,
        IMapper mapper, IHttpContextAccessor contextAccessor, IImageService imageService, IMemoryCache memoryCache)
    {
        _contextAccessor = contextAccessor;
        _imageService = imageService;
        _memoryCache = memoryCache;
        (_userRepository, _mapper) =
            (userRepository, mapper);
    }

    public async Task<UserModel> GetCurrentUser()
    {
        var id = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        if (_memoryCache.TryGetValue(id, out User user)) return _mapper.Map<UserModel>(user);
        user = await _userRepository.Query()
            .Include(user => user.Articles)
            .ThenInclude(article => article.Tags)
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(u => u.Id == id);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(3));
        _memoryCache.Set(id, user, cacheEntryOptions);
        return _mapper.Map<UserModel>(user);
    }
    public async Task<UserModel> GetUserByNickName(string nickName)
    {
        if (!_memoryCache.TryGetValue(nickName, out User user))
        {
            return _mapper.Map<UserModel>(user);
        }
        user = await _userRepository.Query()
            .Include(user => user.Articles
                .Where(article => article.Status == ArticleStatuses.Published))
            .ThenInclude(article => article.Tags)
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(u => u.NickName == nickName);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), nickName);
        }
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(3));
        _memoryCache.Set(nickName, user, cacheEntryOptions);
        return _mapper.Map<UserModel>(user);
    }
    public async Task<UserModel> UpdateUserData(UpdateUserDataDto userDataDto)
    {
        var userId = _contextAccessor.HttpContext!.User.GetCurrentUserId();
        var user = await _userRepository.GetByIdAsync(userId);
        _memoryCache.Remove(userId);
        _memoryCache.Remove(user.NickName);
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
        _memoryCache.Remove(user.Id);
        _memoryCache.Remove(user.NickName);
        await _imageService.SetAvatar(user, avatar);
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserModel>(user);
    }

    public Task<UserModel> AddComplain(UpdateUserComplainDto complainDto)
    {
        throw new NotImplementedException();
    }
}