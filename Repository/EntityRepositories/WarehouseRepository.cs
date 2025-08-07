using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using BusinessLogic.Common;

namespace Repository.EntityRepositories
{
    public class WarehouseRepository : Repository<Warehouse, Warehouse.UpdatableData>, IWarehouseRepository
    {
        public WarehouseRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Warehouse> AddAsync(Warehouse warehouse)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO dbo.Warehouses (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)",
                warehouse,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", warehouse.Name);
                    cmd.Parameters.AddWithValue("@Description", warehouse.Description);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new Warehouse(newId, warehouse.Name, warehouse.Description, new List<Shelve>(), warehouse.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Warehouse> UpdateAsync(Warehouse warehouse)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE dbo.Warehouses SET Name = @Name, Description = @Description WHERE Id = @Id",
                warehouse,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", warehouse.Id);
                    cmd.Parameters.AddWithValue("@Name", warehouse.Name);
                    cmd.Parameters.AddWithValue("@Description", warehouse.Description);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el almacén con Id {warehouse.Id}");

            return warehouse;
        }

        #endregion

        #region Delete

        public async Task<Warehouse> DeleteAsync(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse), "El almacén no puede ser nulo.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE dbo.Warehouses SET IsDeleted = 1 WHERE Id = @Id",
                warehouse,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", warehouse.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el almacén con Id {warehouse.Id}");

            return warehouse;
        }

        #endregion

        #region GetAll

        public async Task<List<Warehouse>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Name", "Description" };

            return await ExecuteReadAsync(
                baseQuery: @"SELECT w.Id, w.Name, w.Description, 
                                    s.Id AS ShelveId, s.Name AS ShelveName, s.Description AS ShelveDescription 
                             FROM Warehouses w
                             LEFT JOIN Shelves s ON s.WarehouseId = w.Id AND s.IsDeleted = 0",
                map: reader =>
                {
                    var warehouses = new Dictionary<int, Warehouse>();

                    while (reader.Read())
                    {
                        var warehouseId = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!warehouses.TryGetValue(warehouseId, out var warehouse))
                        {
                            warehouse = WarehouseMapper.FromReader(reader);
                            warehouse.Shelves = new List<Shelve>();
                            warehouses.Add(warehouseId, warehouse);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("ShelveId")))
                        {
                            var shelve = ShelveMapper.FromReaderForWarehouses(reader);
                            warehouse.Shelves.Add(shelve);
                        }
                    }

                    return warehouses.Values.ToList();
                },
                options: options,
                tableAlias: "w",
                allowedFilterColumns: allowedFilters
            );
        }

        #endregion

        #region GetById

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                @"SELECT w.Id, w.Name, w.Description, 
                                    s.Id AS ShelveId, s.Name AS ShelveName, s.Description AS ShelveDescription 
                             FROM Warehouses w
                             LEFT JOIN Shelves s ON s.WarehouseId = w.Id AND s.IsDeleted = 0
                             WHERE w.Id = @Id",
                map: reader =>
                {
                    Warehouse? warehouse = null;
                    while (reader.Read())
                    {
                        if (warehouse == null)
                            warehouse = WarehouseMapper.FromReader(reader);

                        if (!reader.IsDBNull(reader.GetOrdinal("ShelveId")))
                        {
                            var shelve = ShelveMapper.FromReaderForWarehouses(reader);
                            if (shelve != null)
                                warehouse.Shelves.Add(shelve);
                        }
                    }
                    return warehouse;
                },
                options: new QueryOptions(),
                tableAlias:"w",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }

        public async Task<Warehouse?> GetByNameAsync(string name)
        {
            return await ExecuteReadAsync(
                 @"SELECT w.Id, w.Name, w.Description,
                                    s.Id AS ShelveId, s.Name AS ShelveName, s.Description AS ShelveDescription 
                             FROM Warehouses w
                             LEFT JOIN Shelves s ON s.WarehouseId = w.Id AND s.IsDeleted = 0
                             WHERE w.Name = @Name",
                map: reader =>
                {
                    Warehouse? warehouse = null;
                    while (reader.Read())
                    {
                        if (warehouse == null)
                            warehouse = WarehouseMapper.FromReader(reader);

                        var shelve = ShelveMapper.FromReaderForWarehouses(reader);
                        if (shelve != null)
                            warehouse.Shelves.Add(shelve);
                    }
                    return warehouse;
                },
                options: new QueryOptions(),
                tableAlias: "w",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
            );
        }

        #endregion
    }
}
