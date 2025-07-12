using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Domain
{
    public class Image
    {
        public int Id { get; private set; }
        public string EntityType { get; private set; }
        public int EntityId { get; private set; }
        public string FileName { get; private set; }
        public string BlobName { get; private set; }
        public string ContentType { get; private set; }
        public long Size { get; private set; }
        public DateTime UploadDate { get; private set; }

        public Image(int id, string entityType, int entityId, string fileName,
                     string blobName, string contentType, long size, DateTime uploadDate)
        {
            Id = id;
            EntityType = entityType;
            EntityId = entityId;
            FileName = fileName;
            BlobName = blobName;
            ContentType = contentType;
            Size = size;
            UploadDate = uploadDate;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(EntityType))
                throw new ArgumentException("El tipo de entidad es requerido.", nameof(EntityType));

            if (EntityId <= 0)
                throw new ArgumentException("El ID de entidad debe ser mayor a 0.", nameof(EntityId));

            if (string.IsNullOrWhiteSpace(FileName))
                throw new ArgumentException("El nombre del archivo es requerido.", nameof(FileName));

            if (Size <= 0)
                throw new ArgumentException("El tamaño del archivo debe ser mayor a 0.", nameof(Size));
        }
    }
}
