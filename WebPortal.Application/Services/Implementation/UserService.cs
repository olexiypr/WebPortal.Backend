using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebPortal.Application.Dtos;
using WebPortal.Application.Dtos.Article;
using WebPortal.Application.Dtos.User;
using WebPortal.Application.Models;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain;
using WebPortal.Domain.User;
using WebPortal.Persistence.Exceptions;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Recommendation> _recommendationRepository;
    private readonly IRecommendationService _recommendationService;
    private readonly IMapper _mapper;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public UserService(IRepository<User> userRepository, IRecommendationService recommendationService, 
        IRepository<Tag> tagRepository, IRepository<Recommendation> recommendationRepository,
        IMapper mapper, IWebHostEnvironment webHostEnvironment) =>
        (_userRepository, _recommendationService, _tagRepository,_recommendationRepository ,_mapper, _webHostEnvironment) = 
        (userRepository, recommendationService, tagRepository,recommendationRepository, mapper, webHostEnvironment);
    
    public async Task<UserModel> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.Query()
            .Include(user => user.Articles)
            .ThenInclude(article => article.Tags)
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(u => u.Id == id);
        /*if (user.Avatar!.Length > 0)
        {
            user.Avatar = GetAvatarLink(user.Id);
        }*/
        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
    }

    private string GetAvatarLink(Guid userId)
    {
        return "https://localhost:7150/api/Image/" + userId;
    }
    public async Task<UserModel> GetUserByNickName(string nickName)
    {
        var user = await _userRepository.Query()
            .Include(user => user.Articles)
            .ThenInclude(article => article.Tags)
            .Include(user => user.Recommendation)
            .FirstOrDefaultAsync(u => u.NickName == nickName);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), nickName);
        }
        /*if (user.Avatar!.Length > 0)
        {
            user.Avatar = GetAvatarLink(user.Id);
        }*/
        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
    }
    
    public async Task<UserModel> UpdateUserData(UpdateUserDataDto userDataDto)
    {
        var user = await _userRepository.Query()
            .FirstOrDefaultAsync(user => user.NickName == userDataDto.NickName);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), userDataDto.NickName);
        }

        if (userDataDto.ChangedNickName != null && 
            !await _userRepository.Query().AnyAsync(user => user.NickName == userDataDto.ChangedNickName))
        {
            user.NickName = userDataDto.ChangedNickName;
        }

        user.Name = userDataDto.Name ?? user.Name;
        user.Description = userDataDto.Description ?? user.Description;
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> UpdateUserPhoto(IFormFile avatar, string nickName)
    {
        if (avatar.Length <= 0)
        {
            throw new IOException(nameof(UpdateUserPhotoDto));
        }
        var user = await _userRepository.Query().FirstOrDefaultAsync(user => user.NickName == nickName);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), nickName);
        }
        var path = $"{user.Id}";
        try
        {
            await using var fileStream = new FileStream(_webHostEnvironment.ContentRootPath + "/Avatars" + path, FileMode.Create);
            await avatar.CopyToAsync(fileStream);
            user.Avatar = path;
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw;
        }
        await _userRepository.SaveChangesAsync();
        return _mapper.Map<UserModel>(user);
    }

    public Task<UserModel> AddComplain(UpdateUserComplainDto complainDto)
    {
        throw new NotImplementedException();
    }
}