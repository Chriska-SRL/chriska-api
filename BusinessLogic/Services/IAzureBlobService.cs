using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public interface IAzureBlobService
    {
        string UploadBlob(string blobName, IFormFile file);
        string GetBlobUrl(string blobName);
        bool DeleteBlob(string blobName);
    }
}