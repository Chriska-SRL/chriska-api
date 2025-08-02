using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            string extension = Path.GetExtension(file.FileName);
            string blobName = $"{fileName}{extension}";

            var blobClient = containerClient.GetBlobClient(blobName);

            // Eliminar el archivo anterior si existe
            await blobClient.DeleteIfExistsAsync();

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                }
            );

            return blobClient.Uri.ToString();
        }


        public async Task DeleteFileAsync(string blobUrl, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            string blobName = Path.GetFileName(new Uri(blobUrl).LocalPath);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}
