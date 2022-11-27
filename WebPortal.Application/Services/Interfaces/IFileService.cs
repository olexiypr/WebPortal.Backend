namespace WebPortal.Application.Services.Interfaces;

public interface IFileService
{
    public Task<string> UploadImageAsync(Stream fileStream, string fileName);
    public Task<string> DeleteImageAsync(Stream fileStream, string fileName);
}