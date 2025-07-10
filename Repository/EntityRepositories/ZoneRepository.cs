using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        public ZoneRepository(string connectionString, ILogger<Zone> logger)
            : base(connectionString, logger)
        {
        }

        public Zone Add(Zone entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var sql = @"
                    INSERT INTO Zones 
                        (Name, Description, DeliveryDays, RequestDays, Image,
                         CreatedAt, CreatedBy, CreatedLocation)
                    OUTPUT INSERTED.Id
                    VALUES
                        (@Name, @Description, @DeliveryDays, @RequestDays, @Image,
                         @CreatedAt, @CreatedBy, @CreatedLocation)";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Description", entity.Description);
                command.Parameters.AddWithValue("@DeliveryDays", entity.DeliveryDays);
                command.Parameters.AddWithValue("@RequestDays", entity.RequestDays);
                command.Parameters.AddWithValue("@Image", entity.Image);
                command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
                command.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CreatedLocation", entity.CreatedLocation ?? (object)DBNull.Value);

                int insertedId = (int)command.ExecuteScalar();
                entity.Id = insertedId;

                return entity;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al insertar zona.");
                throw new ApplicationException("Error al insertar zona.", ex);
            }
        }

        public Zone Update(Zone entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var sql = @"
                    UPDATE Zones
                    SET 
                        Name = @Name,
                        Description = @Description,
                        DeliveryDays = @DeliveryDays,
                        RequestDays = @RequestDays,
                        Image = @Image,
                        UpdatedAt = @UpdatedAt,
                        UpdatedBy = @UpdatedBy,
                        UpdatedLocation = @UpdatedLocation
                    WHERE Id = @Id AND DeletedAt IS NULL";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Description", entity.Description);
                command.Parameters.AddWithValue("@DeliveryDays", entity.DeliveryDays);
                command.Parameters.AddWithValue("@RequestDays", entity.RequestDays);
                command.Parameters.AddWithValue("@Image", entity.Image);
                command.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt ?? DateTime.UtcNow);
                command.Parameters.AddWithValue("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UpdatedLocation", entity.UpdatedLocation ?? (object)DBNull.Value);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                    throw new ApplicationException($"No se encontró ninguna zona activa con el ID {entity.Id}");

                return entity;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al actualizar zona.");
                throw new ApplicationException("Error al actualizar zona.", ex);
            }
        }

        public Zone? Delete(int id)
        {
            try
            {
                Zone? existing = GetById(id);
                if (existing == null) return null;

                using var connection = CreateConnection();
                connection.Open();

                var sql = @"
                    UPDATE Zones
                    SET 
                        DeletedAt = @DeletedAt,
                        DeletedBy = @DeletedBy,
                        DeletedLocation = @DeletedLocation
                    WHERE Id = @Id";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DeletedAt", existing.DeletedAt ?? DateTime.UtcNow);
                command.Parameters.AddWithValue("@DeletedBy", existing.DeletedBy ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DeletedLocation", existing.DeletedLocation ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
                return existing;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al realizar soft delete de zona.");
                throw new ApplicationException("Error al eliminar zona.", ex);
            }
        }

        public List<Zone> GetAll()
        {
            var zones = new List<Zone>();
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var sql = @"
                    SELECT Id, Name, Description, DeliveryDays, RequestDays, Image,
                           CreatedAt, CreatedBy, CreatedLocation,
                           UpdatedAt, UpdatedBy, UpdatedLocation,
                           DeletedAt, DeletedBy, DeletedLocation
                    FROM Zones 
                    WHERE DeletedAt IS NULL";

                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    zones.Add(ZoneMapper.FromReader(reader));
                }
                return zones;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las zonas.");
                throw new ApplicationException("Error al obtener todas las zonas.", ex);
            }
        }

        public Zone? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var sql = @"
                    SELECT Id, Name, Description, DeliveryDays, RequestDays, Image,
                           CreatedAt, CreatedBy, CreatedLocation,
                           UpdatedAt, UpdatedBy, UpdatedLocation,
                           DeletedAt, DeletedBy, DeletedLocation
                    FROM Zones
                    WHERE Id = @Id AND DeletedAt IS NULL";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                return reader.Read() ? ZoneMapper.FromReader(reader) : null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
        }
    }
}
