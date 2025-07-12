// BusinessLogic/Services/AzureBlobService.cs
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
            _blobServiceClient = new BlobServiceClient(connectionString);
            _logger = logger;

            // Crear container si no existe
            EnsureContainerExists();
        }

        public string UploadBlob(string blobName, IFormFile file)
        {
            try
            {
                var blobClient = GetBlobClient(blobName);

                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                using var stream = file.OpenReadStream();
                blobClient.Upload(stream, new BlobUploadOptions
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

        public string GetBlobUrl(string blobName)
        {
            try
            {
                var blobClient = GetBlobClient(blobName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener URL del blob {BlobName}", blobName);
                throw new ApplicationException($"Error al obtener la URL de la imagen: {ex.Message}", ex);
            }
        }

        public bool DeleteBlob(string blobName)
        {
            try
            {
                var blobClient = GetBlobClient(blobName);
                var response = blobClient.DeleteIfExists();
                return response.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar blob {BlobName}", blobName);
                throw new ApplicationException($"Error al eliminar la imagen: {ex.Message}", ex);
            }
        }

        private BlobClient GetBlobClient(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            return containerClient.GetBlobClient(blobName);
        }

        private void EnsureContainerExists()
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                containerClient.CreateIfNotExists(PublicAccessType.Blob);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear container {ContainerName}", _containerName);
                throw new ApplicationException($"Error al configurar el almacenamiento: {ex.Message}", ex);
            }
        }
    }
}