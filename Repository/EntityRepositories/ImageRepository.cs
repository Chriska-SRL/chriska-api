using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public async Task<Image> AddAsync(Image entity)
        {
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();

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

                int insertedId = (int)await command.ExecuteScalarAsync();

                return new Image(insertedId, entity.EntityType, entity.EntityId, entity.FileName,
                                 entity.BlobName, entity.ContentType, entity.Size, entity.UploadDate);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error al insertar imagen.", ex);
            }
        }

        public async Task<Image?> GetByEntityTypeAndIdAsync(string entityType, int entityId)
        {
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();

                using var command = new SqlCommand(@"
                    SELECT Id, EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate
                    FROM Images
                    WHERE EntityType = @EntityType AND EntityId = @EntityId", connection);

                command.Parameters.AddWithValue("@EntityType", entityType);
                command.Parameters.AddWithValue("@EntityId", entityId);

                using var reader = await command.ExecuteReaderAsync();
                return await reader.ReadAsync() ? ImageMapper.FromReader(reader) : null;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error al obtener imagen.", ex);
            }
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();

                using var command = new SqlCommand(@"
                    SELECT Id, EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate
                    FROM Images
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                return await reader.ReadAsync() ? ImageMapper.FromReader(reader) : null;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error al obtener imagen por ID.", ex);
            }
        }

        public async Task<Image?> DeleteAsync(int id)
        {
            try
            {
                var existing = await GetByIdAsync(id);
                if (existing == null) return null;

                await using var connection = CreateConnection();
                await connection.OpenAsync();

                using var command = new SqlCommand("DELETE FROM Images WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                await command.ExecuteNonQueryAsync();

                return existing;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error al eliminar imagen.", ex);
            }
        }

        public async Task<List<Image>> GetAllAsync()
        {
            var images = new List<Image>();
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();

                using var command = new SqlCommand(@"
                    SELECT Id, EntityType, EntityId, FileName, BlobName, ContentType, Size, UploadDate
                    FROM Images
                    ORDER BY UploadDate DESC", connection);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    images.Add(ImageMapper.FromReader(reader));
                }
                return images;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error al obtener todas las imágenes.", ex);
            }
        }
    }
}
