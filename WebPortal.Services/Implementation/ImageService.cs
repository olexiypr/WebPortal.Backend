using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebPortal.Domain.User;
using WebPortal.Persistence.Infrastructure;
using WebPortal.Services.Interfaces;

namespace WebPortal.Services.Implementation;

public class ImageService : IImageService
{
    private readonly IRepository<User> _userRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const string PathToAvatars = "/../../Avatars/";
    private const string CurrentUrlToEndpoint = "https://localhost:7150/api/Image/";
    public ImageService(IRepository<User> userRepository, IWebHostEnvironment environment, IWebHostEnvironment webHostEnvironment)
    {
        _userRepository = userRepository;
        _environment = environment;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<FileStream?> GetImageByUserId(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        var path = _environment.WebRootPath + PathToAvatars + user.Id;
        var file = File.Open(path, FileMode.Open);
        return file;
    }

    public async Task SetAvatar(User user, IFormFile avatar)
    {
        if (avatar.Length <= 0)
        {
            throw new IOException(nameof(avatar));
        }

        
        var path = $"{_webHostEnvironment}{PathToAvatars}{user.Id}";
        try
        {
            await using var fileStream = new FileStream(path, FileMode.Create);
            await avatar.CopyToAsync(fileStream);
            user.Avatar = CurrentUrlToEndpoint + user.Id.ToString();
        }
        catch (IOException e)
        {
            Console.WriteLine(e);
            throw new IOException(nameof(avatar));
        }
    }

    public void ValidateImage(IFormFile avatar)
    {
        
    }
}