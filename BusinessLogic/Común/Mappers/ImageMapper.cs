// BusinessLogic/Común/Mappers/ImageMapper.cs
using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsImage;

namespace BusinessLogic.Común.Mappers
{
    public static class ImageMapper
    {
        public static ImageResponse ToResponse(Image image, string imageUrl)
        {
            return new ImageResponse
            {
                Id = image.Id,
                EntityType = image.EntityType,
                EntityId = image.EntityId,
                FileName = image.FileName,
                ImageUrl = imageUrl,
                ContentType = image.ContentType,
                Size = image.Size,
                UploadDate = image.UploadDate
            };
        }
    }
}