using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.Repository;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;


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

        public ImageResponse UploadImage(string entityType, int entityId, IFormFile file)
        {
            // Validaciones
            if (!IsValidEntityType(entityType))
                throw new ArgumentException("Tipo de entidad no válido.", nameof(entityType));

            if (!IsValidImageFile(file))
                throw new ArgumentException("Archivo de imagen no válido.", nameof(file));

            // Eliminar imagen existente si hay una
            var existingImage = _imageRepository.GetByEntityTypeAndId(entityType, entityId);
            if (existingImage != null)
            {
                _azureBlobService.DeleteBlob(existingImage.BlobName);
                _imageRepository.Delete(existingImage.Id);
            }

            // Crear nueva imagen
            var blobName = $"{entityType}/{entityId}/{Guid.NewGuid()}.{GetFileExtension(file.FileName)}";
            var imageUrl = _azureBlobService.UploadBlob(blobName, file);

            var newImage = new Image(0, entityType, entityId, file.FileName, blobName, file.ContentType, file.Length, DateTime.UtcNow);
            newImage.Validate();

            var added = _imageRepository.Add(newImage);
            return ImageMapper.ToResponse(added, imageUrl);
        }

        public ImageResponse? GetImage(string entityType, int entityId)
        {
            if (!IsValidEntityType(entityType))
                throw new ArgumentException("Tipo de entidad no válido.", nameof(entityType));

            var image = _imageRepository.GetByEntityTypeAndId(entityType, entityId);
            if (image == null) return null;

            var imageUrl = _azureBlobService.GetBlobUrl(image.BlobName);
            return ImageMapper.ToResponse(image, imageUrl);
        }

        public bool DeleteImage(string entityType, int entityId)
        {
            if (!IsValidEntityType(entityType))
                throw new ArgumentException("Tipo de entidad no válido.", nameof(entityType));

            var image = _imageRepository.GetByEntityTypeAndId(entityType, entityId);
            if (image == null) return false;

            _azureBlobService.DeleteBlob(image.BlobName);
            _imageRepository.Delete(image.Id);
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