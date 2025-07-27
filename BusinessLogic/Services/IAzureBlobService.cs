using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public interface IAzureBlobService
    {
        Task<string> UploadBlobAsync(string blobName, IFormFile file);
        Task<string> GetBlobUrlAsync(string blobName);
        Task<bool> DeleteBlobAsync(string blobName);
    }
}