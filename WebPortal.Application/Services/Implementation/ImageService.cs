using Microsoft.AspNetCore.Hosting;
using WebPortal.Application.Services.Interfaces;
using WebPortal.Domain.User;
using WebPortal.Persistence.Infrastructure;

namespace WebPortal.Application.Services.Implementation;

public class ImageService : IImageService
{
    private readonly IRepository<User> _userRepository;
    private readonly IWebHostEnvironment _environment;
    public const string PathToAvatars = "/../../Avatars/";
    public ImageService(IRepository<User> userRepository, IWebHostEnvironment environment)
    {
        _userRepository = userRepository;
        _environment = environment;
    }

    public async Task<FileStream?> GetImageByUserId(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        var path = _environment.WebRootPath + PathToAvatars + user.Id;
        var file = File.Open(path, FileMode.Open);
        return file;
    }
}