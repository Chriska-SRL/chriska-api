using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Common;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ShelveRepository : Repository<Shelve, Shelve.UpdatableData>, IShelveRepository
    {
        public ShelveRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public async Task<Shelve> AddAsync(Shelve shelve)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO dbo.Shelves (Name, Description, WarehouseId) OUTPUT INSERTED.Id VALUES (@Name, @Description, @WarehouseId)",
                shelve,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", shelve.Name);
                    cmd.Parameters.AddWithValue("@Description", shelve.Description);
                    cmd.Parameters.AddWithValue("@WarehouseId", shelve.Warehouse.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new Shelve(newId, shelve.Name, shelve.Description, shelve.Warehouse, shelve.AuditInfo);
        }


        public async Task<Shelve> DeleteAsync(Shelve shelve)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE dbo.Shelves SET IsDeleted = 1 WHERE Id = @Id",
                shelve,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", shelve.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la estantería con Id {shelve.Id}");

            return shelve;
        }

        public async Task<List<Shelve>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Name", "Description", "WarehouseId" };

            return await ExecuteReadAsync(
                baseQuery: @"SELECT s.Id, s.Name, s.Description, s.WarehouseId,  
                                    w.Name AS WarehouseName, w.Description AS WarehouseDescription
                             FROM Shelves s
                             INNER JOIN Warehouses w ON s.WarehouseId = w.Id",
                map: reader =>
                {
                    var list = new List<Shelve>();
                    while (reader.Read())
                    {
                        list.Add(ShelveMapper.FromReader(reader));
                    }
                    return list;
                },
                options: options,
                tableAlias: "s",
                allowedFilterColumns: allowedFilters
            );
        }

        public async Task<Shelve?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery:  @"SELECT s.Id, s.Name, s.Description,  
                                    w.Id AS WarehouseId, w.Name AS WarehouseName, w.Description AS WarehouseDescription
                             FROM Shelves s
                             INNER JOIN Warehouses w ON s.WarehouseId = w.Id
                             WHERE s.Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return ShelveMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                tableAlias: "s",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }

        public async Task<Shelve?> GetByNameAsync(string name)
        {
            return await ExecuteReadAsync(
                baseQuery: @"SELECT s.Id, s.Name, s.Description,  
                                    w.Id AS WarehouseId, w.Name AS WarehouseName, w.Description AS WarehouseDescription
                             FROM Shelves s
                             INNER JOIN Warehouses w ON s.WarehouseId = w.Id
                             WHERE s.Name = @Name",
                map: reader =>
                {
                    if (reader.Read())
                        return ShelveMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                tableAlias: "s",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
            );
        }

        public async Task<Shelve> UpdateAsync(Shelve shelve)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE dbo.Shelves SET Name = @Name, Description = @Description, WarehouseId = @WarehouseId WHERE Id = @Id",
                shelve,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", shelve.Id);
                    cmd.Parameters.AddWithValue("@Name", shelve.Name);
                    cmd.Parameters.AddWithValue("@Description", shelve.Description);
                    cmd.Parameters.AddWithValue("@WarehouseId", shelve.Warehouse.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la estantería con Id {shelve.Id}");

            return shelve;
        }
    }
}
