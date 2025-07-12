using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(string connectionString, ILogger<Image> logger) : base(connectionString, logger)
        {
        }

        public Image Add(Image entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    INSERT INTO Images (EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate) 
                    OUTPUT INSERTED.Id 
                    VALUES (@EntityType, @EntityId, @FileName, @BlobName, @ContentType, @Size, @UploadDate)", connection);

                command.Parameters.AddWithValue("@EntityType", entity.EntityType);
                command.Parameters.AddWithValue("@EntityId", entity.EntityId);
                command.Parameters.AddWithValue("@FileName", entity.FileName);
                command.Parameters.AddWithValue("@BlobName", entity.BlobName);
                command.Parameters.AddWithValue("@ContentType", entity.ContentType);
                command.Parameters.AddWithValue("@Size", entity.Size);
                command.Parameters.AddWithValue("@UploadDate", entity.UploadDate);

                int insertedId = (int)command.ExecuteScalar();
                return new Image(insertedId, entity.EntityType, entity.EntityId, entity.FileName,
                               entity.BlobName, entity.ContentType, entity.Size, entity.UploadDate);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al insertar imagen.");
                throw new ApplicationException("Error al insertar imagen.", ex);
            }
        }

        public Image? GetByEntityTypeAndId(string entityType, int entityId)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT Id, EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate 
                    FROM Images 
                    WHERE EntityType = @EntityType AND EntityId = @EntityId", connection);

                command.Parameters.AddWithValue("@EntityType", entityType);
                command.Parameters.AddWithValue("@EntityId", entityId);

                using var reader = command.ExecuteReader();
                return reader.Read() ? ImageMapper.FromReader(reader) : null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al obtener imagen por entidad.");
                throw new ApplicationException("Error al obtener imagen.", ex);
            }
        }

        public Image? Delete(int id)
        {
            try
            {
                Image? existing = GetById(id);
                if (existing == null) return null;

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("DELETE FROM Images WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
                return existing;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen.");
                throw new ApplicationException("Error al eliminar imagen.", ex);
            }
        }

        public Image? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT Id, EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate 
                    FROM Images 
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                return reader.Read() ? ImageMapper.FromReader(reader) : null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al obtener imagen por ID.");
                throw new ApplicationException("Error al obtener imagen.", ex);
            }
        }

        public List<Image> GetAll()
        {
            var images = new List<Image>();
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT Id, EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate 
                    FROM Images 
                    ORDER BY UploadDate DESC", connection);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    images.Add(ImageMapper.FromReader(reader));
                }
                return images;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las imágenes.");
                throw new ApplicationException("Error al obtener todas las imágenes.", ex);
            }
        }
    }
}