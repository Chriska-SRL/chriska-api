using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        public ZoneRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Zone> logger) : base(connectionString, logger)
        {
        }

        public Zone Add(Zone entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("INSERT INTO Zones (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)", connection);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Description", entity.Description);

                int insertedId = (int)command.ExecuteScalar();
                return new Zone(insertedId, entity.Name, entity.Description);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al insertar zona.");
                throw new ApplicationException("Error al insertar zona.", ex);
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

                using var command = new SqlCommand("DELETE FROM Zones WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
                return existing;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al eliminar zona.");
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

                using var command = new SqlCommand(@"
                    SELECT z.Id, z.Name, z.Description, i.BlobName
                    FROM Zones z
                    LEFT JOIN Images i ON i.EntityType = 'zones' AND i.EntityId = z.Id
                    ORDER BY z.Id", connection);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var zone = ZoneMapper.FromReader(reader);

                    zones.Add(zone);
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

                using var command = new SqlCommand(@"
                    SELECT z.Id, z.Name, z.Description, i.BlobName
                    FROM Zones z
                    LEFT JOIN Images i ON i.EntityType = 'zones' AND i.EntityId = z.Id
                    WHERE z.Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();

                if (!reader.Read()) return null;

                var zone = ZoneMapper.FromReader(reader);

                return zone;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
        }

        public Zone Update(Zone entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("UPDATE Zones SET Name = @Name, Description = @Description WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", entity.Id);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Description", entity.Description);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                    throw new ApplicationException($"No se encontró ninguna zona con el ID {entity.Id}");

                return GetById(entity.Id) ?? entity;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al actualizar zona.");
                throw new ApplicationException("Error al actualizar zona.", ex);
            }
        }
    }
}