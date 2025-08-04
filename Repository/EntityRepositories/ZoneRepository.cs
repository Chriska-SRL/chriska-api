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

    #region Add
    public async Task<Zone> AddAsync(Zone zone)
    {
        int newId = await ExecuteWriteWithAuditAsync(
            "INSERT INTO Zones (Name, Description, DeliveryDays, RequestDays) " +
            "VALUES (@Name, @Description, @DeliveryDays, @RequestDays); " +
            "SELECT CAST(SCOPE_IDENTITY() AS INT);",
            zone,
            AuditAction.Insert,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", zone.Name);
                cmd.Parameters.AddWithValue("@Description", zone.Description);
                cmd.Parameters.AddWithValue("@DeliveryDays", string.Join(",", zone.DeliveryDays));
                cmd.Parameters.AddWithValue("@RequestDays", string.Join(",", zone.RequestDays));
            },
            async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
        );

        zone.Id = newId;
        return zone;
    }
    #endregion

    #region Update
    public async Task<Zone> UpdateAsync(Zone zone)
    {
        int rows = await ExecuteWriteWithAuditAsync(
            "UPDATE Zones SET Name = @Name, Description = @Description, DeliveryDays = @DeliveryDays, RequestDays = @RequestDays " +
            "WHERE Id = @Id",
            zone,
            AuditAction.Update,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", zone.Id);
                cmd.Parameters.AddWithValue("@Name", zone.Name);
                cmd.Parameters.AddWithValue("@Description", zone.Description);
                cmd.Parameters.AddWithValue("@DeliveryDays", string.Join(",", zone.DeliveryDays));
                cmd.Parameters.AddWithValue("@RequestDays", string.Join(",", zone.RequestDays));
            }
        );

        if (rows == 0)
            throw new InvalidOperationException($"No se pudo actualizar la zona con Id {zone.Id}");

        return zone;
    }
    #endregion



    #region Delete
    public async Task<Zone> DeleteAsync(Zone zone)
    {
        int rows = await ExecuteWriteWithAuditAsync(
            "UPDATE Zones SET IsDeleted = 1 WHERE Id = @Id",
            zone,
            AuditAction.Delete,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", zone.Id);
            }
        );

        if (rows == 0)
            throw new InvalidOperationException($"No se pudo eliminar la zona con Id {zone.Id}");

        return zone;
    }
    #endregion

    #region GetAll
    public async Task<List<Zone>> GetAllAsync(QueryOptions options)
    {
        var allowedFilters = new[] { "Name" ,"Description", "DeliveryDays", "RequestDays" };

        return await ExecuteReadAsync(
            baseQuery: "SELECT z.* FROM Zones z",
            map: reader =>
            {
                var zones = new List<Zone>();
                while (reader.Read())
                {
                    var zone = ZoneMapper.FromReader(reader);
                    if (zone != null)
                        zones.Add(zone);
                }
                return zones;
            },
            options: options,
            allowedFilterColumns: allowedFilters,
            tableAlias: "z"
        );
    }
    #endregion

    #region GetById
    public async Task<Zone?> GetByIdAsync(int id)
    {
        return await ExecuteReadAsync(
            baseQuery: "SELECT z.* FROM Zones z WHERE z.Id = @Id",
            map: reader =>
            {
                if (reader.Read())
                {
                    return ZoneMapper.FromReader(reader);
                }
                return null;
            },
            options: new QueryOptions(),
            tableAlias: "z",
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", id);
            }
        );
    }
    #endregion

    public async Task<string> UpdateImageUrlAsync(Zone zone, string imageUrl)
    {
        int rows = await ExecuteWriteWithAuditAsync(
            "UPDATE Zones SET ImageUrl = @ImageUrl WHERE Id = @Id",
            zone,
            AuditAction.Update,
            configureCommand: cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", zone.Id);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
            }
        );

        if (rows == 0)
            throw new InvalidOperationException($"No se pudo actualizar la imagen de la zona con Id {zone.Id}");

        return imageUrl;
    }


}
