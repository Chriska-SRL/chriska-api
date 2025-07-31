using BusinessLogic.Common;
using BusinessLogic.Común;

namespace BusinessLogic.Domain
{
    public class Image : IEntity<Image.UpdatableData>, IAuditable
    {
        public int Id { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string FileName { get; set; }
        public string BlobName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public AuditInfo AuditInfo { get; set; } = new AuditInfo();

        public Image(int id, string entityType, int entityId, string fileName,
                     string blobName, string contentType, long size, DateTime uploadDate, AuditInfo auditInfo)
        {
            Id = id;
            EntityType = entityType;
            EntityId = entityId;
            FileName = fileName;
            BlobName = blobName;
            ContentType = contentType;
            Size = size;
            UploadDate = uploadDate;
            AuditInfo = auditInfo ?? new AuditInfo();
            Validate();
        }

        public Image(int id)
        {
            Id = id;
            EntityType = "TemporalType";
            EntityId = 0;
            FileName = "default.jpg";
            BlobName = "default_blob";
            ContentType = "image/jpeg";
            Size = 0;
            UploadDate = DateTime.UtcNow;

            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(EntityType))
                throw new ArgumentNullException(nameof(EntityType), "El tipo de entidad es obligatorio.");
            if (EntityId <= 0)
                throw new ArgumentOutOfRangeException(nameof(EntityId), "El ID de entidad debe ser mayor a 0.");

            if (string.IsNullOrWhiteSpace(FileName))
                throw new ArgumentNullException(nameof(FileName), "El nombre del archivo es obligatorio.");

            if (Size <= 0)
                throw new ArgumentOutOfRangeException(nameof(Size), "El tamaño del archivo debe ser mayor a 0.");
        }

        public void Update(UpdatableData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Los datos de actualización no pueden ser nulos.");

            EntityType = data.EntityType ?? EntityType;
            EntityId = data.EntityId > 0 ? data.EntityId : EntityId;
            FileName = data.FileName ?? FileName;
            BlobName = data.BlobName ?? BlobName;
            ContentType = data.ContentType ?? ContentType;
            Size = data.Size > 0 ? data.Size : Size;
            UploadDate = data.UploadDate ?? UploadDate;
            AuditInfo = data.AuditInfo ?? AuditInfo;

            Validate();
        }

        public void MarkAsDeleted(int? userId, Location? location)
        {
            throw new NotImplementedException();
        }

        public class UpdatableData
        {
            public string? EntityType { get; set; } = string.Empty;
            public int EntityId { get; set; }
            public string? FileName { get; set; } = string.Empty;
            public string? BlobName { get; set; } = string.Empty;
            public string? ContentType { get; set; } = string.Empty;
            public long Size { get; set; }
            public DateTime? UploadDate { get; set; }
            public AuditInfo AuditInfo { get; set; } = new AuditInfo();
        }
    }
}
