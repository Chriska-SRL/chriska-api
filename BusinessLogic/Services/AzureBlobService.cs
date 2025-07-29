using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(IConfiguration configuration, ILogger<AzureBlobService> logger)
        {
            var connectionString = configuration.GetConnectionString("AzureStorage");
            _containerName = configuration["AzureStorage:ContainerName"] ?? "images";
            _blobServiceClient = null;//new BlobServiceClient(connectionString);
            _logger = logger;
        }

        public async Task<string> UploadBlobAsync(string blobName, IFormFile file)
        {
            try
            {
                var blobClient = await GetBlobClientAsync(blobName);

                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, new BlobUploadOptions
                {
                    HttpHeaders = blobHttpHeaders
                });

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir blob {BlobName}", blobName);
                throw new ApplicationException($"Error al subir la imagen: {ex.Message}", ex);
            }
        }

        public async Task<string> GetBlobUrlAsync(string blobName)
        {
            try
            {
                var blobClient = await GetBlobClientAsync(blobName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener URL del blob {BlobName}", blobName);
                throw new ApplicationException($"Error al obtener la URL de la imagen: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteBlobAsync(string blobName)
        {
            try
            {
                var blobClient = await GetBlobClientAsync(blobName);
                var response = await blobClient.DeleteIfExistsAsync();
                return response.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar blob {BlobName}", blobName);
                throw new ApplicationException($"Error al eliminar la imagen: {ex.Message}", ex);
            }
        }

        private async Task<BlobClient> GetBlobClientAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            return containerClient.GetBlobClient(blobName);
        }
    }
}
