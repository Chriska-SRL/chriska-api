using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        public ZoneRepository(string connectionString, ILogger<Zone> logger) : base(connectionString, logger) { }

        public Zone Add(Zone zone)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    INSERT INTO Zones (Name, Description)
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Description)", connection);

                command.Parameters.AddWithValue("@Name", zone.Name);
                command.Parameters.AddWithValue("@Description", zone.Description);

                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("No se pudo insertar la zona.");

                int zoneId = (int)result;

                InsertZoneDays(connection, zoneId, zone.DeliveryDays, "Zones_DeliveryDays");
                InsertZoneDays(connection, zoneId, zone.RequestDays, "Zones_RequestDays");

                return new Zone(zoneId, zone.Name, zone.Description, zone.DeliveryDays, zone.RequestDays);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al insertar zona.");
                throw new ApplicationException("Error SQL al insertar zona.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar zona.");
                throw new ApplicationException("Ocurrió un error inesperado al insertar zona.", ex);
            }
        }

        public Zone? Delete(int id)
        {
            try
            {
                var existing = GetById(id);
                if (existing == null)
                {
                    _logger.LogWarning($"Zona con ID {id} no encontrada para eliminar.");
                    return null;
                }

                using var connection = CreateConnection();
                connection.Open();

                DeleteZoneDays(connection, id, "Zones_DeliveryDays");
                DeleteZoneDays(connection, id, "Zones_RequestDays");

                using var command = new SqlCommand("DELETE FROM Zones WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();

                _logger.LogInformation($"Zona con ID {id} eliminada correctamente.");
                return existing;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al eliminar zona.");
                throw new ApplicationException("Error SQL al eliminar zona.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar zona.");
                throw new ApplicationException("Ocurrió un error inesperado al eliminar zona.", ex);
            }
        }

        public List<Zone> GetAll()
        {
            try
            {
                var zones = new List<Zone>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT Id, Name, Description FROM Zones", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    zones.Add(ZoneMapper.FromReader(reader));
                }

                foreach (var zone in zones)
                {
                    zone.DeliveryDays = GetZoneDays(connection, zone.Id, "Zones_DeliveryDays");
                    zone.RequestDays = GetZoneDays(connection, zone.Id, "Zones_RequestDays");
                }

                return zones;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener todas las zonas.");
                throw new ApplicationException("Error SQL al obtener todas las zonas.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener todas las zonas.");
                throw new ApplicationException("Ocurrió un error inesperado al obtener zonas.", ex);
            }
        }

        public Zone? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT Id, Name, Description FROM Zones WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                var zone = ZoneMapper.FromReader(reader);

                zone.DeliveryDays = GetZoneDays(connection, zone.Id, "Zones_DeliveryDays");
                zone.RequestDays = GetZoneDays(connection, zone.Id, "Zones_RequestDays");

                return zone;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener zona por ID.");
                throw new ApplicationException("Error SQL al obtener zona.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener zona.");
                throw new ApplicationException("Ocurrió un error inesperado al obtener zona.", ex);
            }
        }

        public Zone Update(Zone zone)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Zones 
                    SET Name = @Name, Description = @Description 
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", zone.Id);
                command.Parameters.AddWithValue("@Name", zone.Name);
                command.Parameters.AddWithValue("@Description", zone.Description);

                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new InvalidOperationException($"No se encontró la zona con ID {zone.Id} para actualizar.");

                DeleteZoneDays(connection, zone.Id, "Zones_DeliveryDays");
                DeleteZoneDays(connection, zone.Id, "Zones_RequestDays");

                InsertZoneDays(connection, zone.Id, zone.DeliveryDays, "Zones_DeliveryDays");
                InsertZoneDays(connection, zone.Id, zone.RequestDays, "Zones_RequestDays");

                return zone;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al actualizar zona.");
                throw new ApplicationException("Error SQL al actualizar zona.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar zona.");
                throw new ApplicationException("Ocurrió un error inesperado al actualizar zona.", ex);
            }
        }

        // Helpers

        private void InsertZoneDays(SqlConnection connection, int zoneId, List<Day> days, string tableName)
        {
            foreach (var day in days)
            {
                using var cmd = new SqlCommand($"INSERT INTO {tableName} (ZoneId, Day) VALUES (@ZoneId, @Day)", connection);
                cmd.Parameters.AddWithValue("@ZoneId", zoneId);
                cmd.Parameters.AddWithValue("@Day", (int)day);
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteZoneDays(SqlConnection connection, int zoneId, string tableName)
        {
            using var cmd = new SqlCommand($"DELETE FROM {tableName} WHERE ZoneId = @ZoneId", connection);
            cmd.Parameters.AddWithValue("@ZoneId", zoneId);
            cmd.ExecuteNonQuery();
        }

        private List<Day> GetZoneDays(SqlConnection connection, int zoneId, string tableName)
        {
            var days = new List<Day>();
            using var cmd = new SqlCommand($"SELECT Day FROM {tableName} WHERE ZoneId = @ZoneId", connection);
            cmd.Parameters.AddWithValue("@ZoneId", zoneId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int value = reader.GetInt32(0);
                if (Enum.IsDefined(typeof(Day), value))
                    days.Add((Day)value);
            }
            return days;
        }
    }
}
