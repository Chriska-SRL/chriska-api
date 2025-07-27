using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.Repository;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BusinessLogic.SubSystem
{
    public class ImagesSubSystem
    {
        private readonly IImageRepository _imageRepository;
        private readonly IAzureBlobService _azureBlobService;

        public ImagesSubSystem(IImageRepository imageRepository, IAzureBlobService azureBlobService)
        {
            _imageRepository = imageRepository;
            _azureBlobService = azureBlobService;
        }

        public async Task<ImageResponse> UploadImageAsync(string entityType, int entityId, IFormFile file)
        {
            if (!IsValidEntityType(entityType))
                throw new ArgumentException("Tipo de entidad no válido.", nameof(entityType));

            if (!IsValidImageFile(file))
                throw new ArgumentException("Archivo de imagen no válido.", nameof(file));

            var existingImage = await _imageRepository.GetByEntityTypeAndIdAsync(entityType, entityId);
            if (existingImage != null)
            {
                await _azureBlobService.DeleteBlobAsync(existingImage.BlobName);
                await _imageRepository.DeleteAsync(existingImage.Id);
            }

            var blobName = $"{entityType}/{entityId}/{Guid.NewGuid()}.{GetFileExtension(file.FileName)}";
            var imageUrl = await _azureBlobService.UploadBlobAsync(blobName, file);

            var newImage = new Image(
                id: 0,
                entityType: entityType,
                entityId: entityId,
                fileName: file.FileName,
                blobName: blobName,
                contentType: file.ContentType,
                size: file.Length,
                uploadDate: DateTime.UtcNow,
                auditInfo: new AuditInfo()
            );

            newImage.Validate();

            var added = await _imageRepository.AddAsync(newImage);
            return ImageMapper.ToResponse(added, imageUrl);
        }

        public async Task<ImageResponse?> GetImageAsync(string entityType, int entityId)
        {
            if (!IsValidEntityType(entityType))
                throw new ArgumentException("Tipo de entidad no válido.", nameof(entityType));

            var image = await _imageRepository.GetByEntityTypeAndIdAsync(entityType, entityId);
            if (image == null) return null;

            var imageUrl = await _azureBlobService.GetBlobUrlAsync(image.BlobName);
            return ImageMapper.ToResponse(image, imageUrl);
        }

        public async Task<bool> DeleteImageAsync(string entityType, int entityId)
        {
            if (!IsValidEntityType(entityType))
                throw new ArgumentException("Tipo de entidad no válido.", nameof(entityType));

            var image = await _imageRepository.GetByEntityTypeAndIdAsync(entityType, entityId);
            if (image == null) return false;

            await _azureBlobService.DeleteBlobAsync(image.BlobName);
            await _imageRepository.DeleteAsync(image.Id);

            return true;
        }

        private bool IsValidEntityType(string entityType)
        {
            var validTypes = new[] { "zones", "products", "vehicles", "warehouses", "brands" };
            return validTypes.Contains(entityType.ToLower());
        }

        private bool IsValidImageFile(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            return file.Length > 0 &&
                   file.Length <= 8 * 1024 * 1024 && // 8MB max
                   allowedTypes.Contains(file.ContentType);
        }

        private string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.').ToLower();
        }
    }
}
