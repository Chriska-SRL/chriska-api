using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public interface IAzureBlobService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName);
        Task DeleteFileAsync(string blobUrl, string containerName);
    }
}