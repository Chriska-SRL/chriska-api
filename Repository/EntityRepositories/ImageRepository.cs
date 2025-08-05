using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;

namespace Repository.EntityRepositories
{
    public class ImageRepository : Repository<Image, Image.UpdatableData>, IImageRepository
    {
        public ImageRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Image> AddAsync(Image image)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Images (EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate) " +
                "OUTPUT INSERTED.Id VALUES (@EntityType, @EntityId, @FileName, @BlobName, @ContentType, @Size, @UploadDate)",
                image,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@EntityType", image.EntityType);
                    cmd.Parameters.AddWithValue("@EntityId", image.EntityId);
                    cmd.Parameters.AddWithValue("@FileName", image.FileName);
                    cmd.Parameters.AddWithValue("@BlobName", image.BlobName);
                    cmd.Parameters.AddWithValue("@ContentType", image.ContentType);
                    cmd.Parameters.AddWithValue("@Size", image.Size);
                    cmd.Parameters.AddWithValue("@UploadDate", image.UploadDate);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new Image(newId, image.EntityType, image.EntityId, image.FileName, image.BlobName, image.ContentType, image.Size, image.UploadDate, image.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Image> UpdateAsync(Image image)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Images SET EntityType = @EntityType, EntityId = @EntityId, FileName = @FileName, BlobName = @BlobName, ContentType = @ContentType, Size = @Size, UploadDate = @UploadDate " +
                "WHERE Id = @Id",
                image,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", image.Id);
                    cmd.Parameters.AddWithValue("@EntityType", image.EntityType);
                    cmd.Parameters.AddWithValue("@EntityId", image.EntityId);
                    cmd.Parameters.AddWithValue("@FileName", image.FileName);
                    cmd.Parameters.AddWithValue("@BlobName", image.BlobName);
                    cmd.Parameters.AddWithValue("@ContentType", image.ContentType);
                    cmd.Parameters.AddWithValue("@Size", image.Size);
                    cmd.Parameters.AddWithValue("@UploadDate", image.UploadDate);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la imagen con Id {image.Id}");

            return image;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(int imageId)
        {
            // Eliminar la imagen físicamente de la base de datos
            int rows = await ExecuteWriteWithAuditAsync(
                "DELETE FROM Images WHERE Id = @Id",
                null,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", imageId);
                }
            );

            return rows > 0;
        }

        #endregion

        #region GetByEntityTypeAndId

        public async Task<Image?> GetByEntityTypeAndIdAsync(string entityType, int entityId)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM Images WHERE EntityType = @EntityType AND EntityId = @EntityId",
                map: reader =>
                {
                    if (reader.Read())
                        return new Image(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("EntityType")),
                            reader.GetInt32(reader.GetOrdinal("EntityId")),
                            reader.GetString(reader.GetOrdinal("FileName")),
                            reader.GetString(reader.GetOrdinal("BlobName")),
                            reader.GetString(reader.GetOrdinal("ContentType")),
                            reader.GetInt64(reader.GetOrdinal("Size")),
                            reader.GetDateTime(reader.GetOrdinal("UploadDate")),
                            new AuditInfo() // Asumir que AuditInfo es adecuado o hacerlo más avanzado si es necesario
                        );
                    return null;
                },
                options: new QueryOptions(),
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@EntityType", entityType);
                    cmd.Parameters.AddWithValue("@EntityId", entityId);
                }
            );
        }

        #endregion
    }
}
