using Microsoft.AspNetCore.Mvc;
using WebPortal.Application.Services.Interfaces;

namespace WebPortal.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : BaseController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult> GetImageForUser(Guid userId)
    {
        var image = await _imageService.GetImageByUserId(userId);
        return Ok(image);
    }
}