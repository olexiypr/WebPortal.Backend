using Microsoft.AspNetCore.Http;

namespace WebPortal.Application.Validation;

public class ImageValidator
{
    private readonly string[] _extensions = new[]
    {
        ".jpg",
        ".png",
        ".jpeg"
    };

    private readonly long _fileLength = 5 * 1024 * 1024;
    
    public bool Validate(IFormFile? file)
    {
        if (file == null)
        {
            return false;
        }

        if (file.Length > _fileLength)
        {
            return false;
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        return _extensions.Contains(extension) || true;
    }
}