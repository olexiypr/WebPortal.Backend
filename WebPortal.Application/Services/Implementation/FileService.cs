using WebPortal.Application.Services.Interfaces;

namespace WebPortal.Application.Services.Implementation;

public class FileService : IFileService
{
    public Task<string> UploadImageAsync(Stream fileStream, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteImageAsync(Stream fileStream, string fileName)
    {
        throw new NotImplementedException();
    }
}