using BusinessLogic.Domain;
using BusinessLogic.Repository;
using BusinessLogic.Común;
using Microsoft.Data.SqlClient;
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
    INSERT INTO Zones (Name, Description, DeliveryDays, RequestDays)
    OUTPUT INSERTED.Id
    VALUES (@Name, @Description, @DeliveryDays, @RequestDays)";

        return await ExecuteWriteWithAuditAsync(
            insertZoneQuery,
            entity,
            AuditAction.Insert,
            cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", entity.Name);
                cmd.Parameters.AddWithValue("@Description", entity.Description);
                cmd.Parameters.AddWithValue("@DeliveryDays", string.Join(",", entity.DeliveryDays.Select(d => d.ToString())));
                cmd.Parameters.AddWithValue("@RequestDays", string.Join(",", entity.RequestDays.Select(d => d.ToString())));

            },
            async cmd =>
            {
                var zoneId = (int)await cmd.ExecuteScalarAsync();

                return new Zone(zoneId, entity.Name, entity.Description,entity.ImageUrl, entity.DeliveryDays, entity.RequestDays, entity.AuditInfo);
            }
        );
    }

    public async Task<Zone> UpdateAsync(Zone entity)
    {
        const string updateQuery = @"
        UPDATE Zones 
        SET Name = @Name,
            Description = @Description,
            DeliveryDays = @DeliveryDays,
            RequestDays = @RequestDays
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
                cmd.Parameters.AddWithValue("@DeliveryDays", string.Join(",", entity.DeliveryDays.Select(d => d.ToString())));
                cmd.Parameters.AddWithValue("@RequestDays", string.Join(",", entity.RequestDays.Select(d => d.ToString())));
            },
            async cmd =>
            {
                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                    throw new InvalidOperationException($"No se encontró la zona con Id {entity.Id} para actualizar.");

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
    
    public async Task<string> UpdateImageUrlAsync(Zone zone, string imageUrl)
    {
        int rows = await ExecuteWriteWithAuditAsync(
            "UPDATE Zones SET ImageUrl = @ImageUrl WHERE Id = @Id",
            zone,
            AuditAction.Update,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", zone.Id);
                cmd.Parameters.AddWithValue("@ImageUrl", (object?)imageUrl ?? DBNull.Value);
            }
        );

        if (rows == 0)
            throw new InvalidOperationException($"No se pudo actualizar la imagen de la zona con Id {zone.Id}");

        return imageUrl;
    }


}
