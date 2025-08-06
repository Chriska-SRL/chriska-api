using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using BusinessLogic.Common;

namespace Repository.EntityRepositories
{
    public class VehicleCostRepository : Repository<VehicleCost, VehicleCost.UpdatableData>, IVehicleCostRepository
    {
        public VehicleCostRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add
        public async Task<VehicleCost> AddAsync(VehicleCost cost)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO VehicleCosts (VehicleId, Type, Description, Amount, Date) " +
                "VALUES (@VehicleId, @Type, @Description, @Amount, @Date); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                cost,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@VehicleId", cost.Vehicle.Id);
                    cmd.Parameters.AddWithValue("@Type", cost.Type);
                    cmd.Parameters.AddWithValue("@Description", cost.Description);
                    cmd.Parameters.AddWithValue("@Amount", cost.Amount);
                    cmd.Parameters.AddWithValue("@Date", cost.Date);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            cost.Id = newId;
            return cost;
        }
        #endregion

        #region Update
        public async Task<VehicleCost> UpdateAsync(VehicleCost cost)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE VehicleCosts SET Type = @Type, Description = @Description, Amount = @Amount, Date = @Date WHERE Id = @Id",
                cost,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", cost.Id);
                    cmd.Parameters.AddWithValue("@Type", cost.Type);
                    cmd.Parameters.AddWithValue("@Description", cost.Description);
                    cmd.Parameters.AddWithValue("@Amount", cost.Amount);
                    cmd.Parameters.AddWithValue("@Date", cost.Date);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el costo con Id {cost.Id}");

            return cost;
        }
        #endregion

        #region Delete
        public async Task<VehicleCost> DeleteAsync(VehicleCost cost)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE VehicleCosts SET IsDeleted = 1 WHERE Id = @Id",
                cost,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", cost.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el costo con Id {cost.Id}");

            return cost;
        }
        #endregion

        #region GetAll
        public async Task<List<VehicleCost>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Type", "Amount", "Date", "VehicleId" };

            return await ExecuteReadAsync(
                baseQuery: @"
                    SELECT 
                        vc.*,
                        v.Plate, v.Brand, v.Model, v.CrateCapacity
                    FROM VehicleCosts vc
                    INNER JOIN Vehicles v ON vc.VehicleId = v.Id
                    WHERE v.IsDeleted = 0",
                map: reader =>
                {
                    var costs = new List<VehicleCost>();
                    while (reader.Read())
                    {
                        var cost = VehicleCostMapper.FromReader(reader);
                        if (cost != null)
                            costs.Add(cost);
                    }
                    return costs;
                },
                options: options,
                allowedFilterColumns: allowedFilters,
                tableAlias: "vc"
            );
        }
        #endregion

        #region GetById
        public async Task<VehicleCost?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                    SELECT 
                        vc.*, 
                        v.Id AS VehicleId,
                        v.Plate, v.Brand, v.Model, v.CrateCapacity,
                        v.CreatedAt AS VehicleCreatedAt,
                        v.CreatedBy AS VehicleCreatedBy,
                        v.CreatedLocation AS VehicleCreatedLocation,
                        v.UpdatedAt AS VehicleUpdatedAt,
                        v.UpdatedBy AS VehicleUpdatedBy,
                        v.UpdatedLocation AS VehicleUpdatedLocation,
                        v.DeletedAt AS VehicleDeletedAt,
                        v.DeletedBy AS VehicleDeletedBy,
                        v.DeletedLocation AS VehicleDeletedLocation
                    FROM VehicleCosts vc
                    JOIN Vehicles v ON vc.VehicleId = v.Id
                    WHERE vc.Id = @Id
                    ",
                map: reader =>
                {
                    if (reader.Read())
                    {
                        return VehicleCostMapper.FromReader(reader);
                    }
                    return null;
                },
                options: new QueryOptions(),
                tableAlias: "vc",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }
        #endregion


    }
}
