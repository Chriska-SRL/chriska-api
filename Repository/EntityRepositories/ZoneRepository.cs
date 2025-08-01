using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.Común;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;
using Repository.Logging;

namespace Repository.EntityRepositories;

public class ZoneRepository : Repository<Zone, Zone.UpdatableData>, IZoneRepository
{
    public ZoneRepository(string connectionString, AuditLogger auditLogger)
        : base(connectionString, auditLogger) { }

    public async Task<Zone> AddAsync(Zone entity)
    {
        const string insertZoneQuery = @"
            INSERT INTO Zones (Name, Description, ImageUrl)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Description, @ImageUrl)";

        return await ExecuteWriteWithAuditAsync(
            insertZoneQuery,
            entity,
            AuditAction.Insert,
            cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);
                cmd.Parameters.AddWithValue("@ImageUrl", (object?)entity.ImageUrl ?? DBNull.Value);
            },
            async cmd =>
            {
                var zoneId = (int)await cmd.ExecuteScalarAsync();

                using var connection = cmd.Connection!;
                using var transaction = cmd.Transaction!;

                await InsertDaysAsync(zoneId, entity.DeliveryDays, "Entrega", connection, transaction);
                await InsertDaysAsync(zoneId, entity.RequestDays, "Pedido", connection, transaction);

                return new Zone(zoneId, entity.Name, entity.Description, entity.DeliveryDays, entity.RequestDays, entity.AuditInfo);
            }
        );
    }


    public async Task<Zone> UpdateAsync(Zone entity)
    {
        const string updateQuery = @"
        UPDATE Zones 
        SET Name = @Name, Description = @Description, ImageUrl = @ImageUrl,
            UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy, UpdatedLocation = @UpdatedLocation
        WHERE Id = @Id";

        return await ExecuteWriteWithAuditAsync(
            updateQuery,
            entity,
            AuditAction.Update,
            cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);
                cmd.Parameters.AddWithValue("@ImageUrl", (object?)entity.ImageUrl ?? DBNull.Value);
            },
            async cmd =>
            {
                await cmd.ExecuteNonQueryAsync();

                using var connection = cmd.Connection!;
                using var transaction = cmd.Transaction!;

                var deleteDaysCmd = new SqlCommand("DELETE FROM ZoneDays WHERE ZoneId = @ZoneId", connection, transaction);
                deleteDaysCmd.Parameters.AddWithValue("@ZoneId", entity.Id);
                await deleteDaysCmd.ExecuteNonQueryAsync();

                await InsertDaysAsync(entity.Id, entity.DeliveryDays, "Entrega", connection, transaction);
                await InsertDaysAsync(entity.Id, entity.RequestDays, "Pedido", connection, transaction);

                return entity;
            }
        );
    }

    public async Task<Zone> DeleteAsync(Zone entity)
    {
        const string deleteQuery = @"
    UPDATE Zones 
    SET IsDeleted = 1 
    WHERE Id = @Id";

        return await ExecuteWriteWithAuditAsync(
            deleteQuery,
            entity,
            AuditAction.Delete,
            cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", entity.Id);
            },
            async cmd =>
            {
                await cmd.ExecuteNonQueryAsync();
                return entity;
            }
        );
    }


    public async Task<Zone?> GetByIdAsync(int id)
    {
        const string query = "SELECT * FROM Zones WHERE Id = @Id";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return null;

            Zone? zone = null;
            while (await reader.ReadAsync())
            {
                zone = ZoneMapper.FromReader(reader);
            }

   
            return zone;
        }
        catch (Exception ex)
        {
            throw new Exception("Ocurrió un error al obtener la zona.");
        }
    }

    public async Task<List<Zone>> GetAllAsync(QueryOptions options)
    {
        const string query = "SELECT * FROM Zones";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var zones = new List<Zone>();
            while (await reader.ReadAsync())
            {
                var zone = ZoneMapper.FromReader(reader);
                if (zone != null)
                    zones.Add(zone);
            }
     
            return zones;
        }
        catch (Exception ex)
        {
            throw new Exception("Ocurrió un error al obtener las zonas: " + ex.Message, ex);
        }
    }
    
    private async Task InsertDaysAsync(int zoneId, List<Day> days, string type, SqlConnection connection, SqlTransaction transaction)
    {
        const string insertDay = @"
            INSERT INTO ZoneDays (ZoneId, Day, Type)
            VALUES (@ZoneId, @Day, @Type)";

        foreach (var day in days)
        {
            var cmd = new SqlCommand(insertDay, connection, transaction);
            cmd.Parameters.AddWithValue("@ZoneId", zoneId);
            cmd.Parameters.AddWithValue("@Day", day.ToString());
            cmd.Parameters.AddWithValue("@Type", type);
            await cmd.ExecuteNonQueryAsync();
        }
    }

 
}
