using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class VehicleRepository : Repository<Vehicle, Vehicle.UpdatableData>, IVehicleRepository
    {
        public VehicleRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Vehicles (Plate, Brand, Model, CrateCapacity) " +
                "VALUES (@Plate, @Brand, @Model, @CrateCapacity); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                vehicle,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Plate", vehicle.Plate);
                    cmd.Parameters.AddWithValue("@Brand", vehicle.Brand);
                    cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@CrateCapacity", vehicle.CrateCapacity);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            vehicle.Id = newId;
            return vehicle;
        }

        #endregion

        #region Update

        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Vehicles SET Plate = @Plate, Brand = @Brand, Model = @Model, CrateCapacity = @CrateCapacity WHERE Id = @Id",
                vehicle,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", vehicle.Id);
                    cmd.Parameters.AddWithValue("@Plate", vehicle.Plate);
                    cmd.Parameters.AddWithValue("@Brand", vehicle.Brand);
                    cmd.Parameters.AddWithValue("@Model", vehicle.Model);
                    cmd.Parameters.AddWithValue("@CrateCapacity", vehicle.CrateCapacity);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el vehículo con Id {vehicle.Id}");

            return vehicle;
        }

        #endregion

        #region Delete

        public async Task<Vehicle> DeleteAsync(Vehicle vehicle)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Vehicles SET IsDeleted = 1 WHERE Id = @Id",
                vehicle,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", vehicle.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el vehículo con Id {vehicle.Id}");

            return vehicle;
        }

        #endregion

        #region GetAll

        public async Task<List<Vehicle>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Plate", "Brand", "Model" };

            return await ExecuteReadAsync(
                baseQuery: "SELECT v.* FROM Vehicles v",
                map: reader =>
                {
                    var vehicles = new List<Vehicle>();
                    while (reader.Read())
                    {
                        var vehicle = VehicleMapper.FromReader(reader);
                        vehicles.Add(vehicle);
                    }
                    return vehicles;
                },
                options: options,
                allowedFilterColumns: allowedFilters,
                tableAlias: "v"
            );
        }

        #endregion

        #region GetById

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT v.* FROM Vehicles v WHERE v.Id = @Id",
                map: reader =>
                {
                    Vehicle? vehicle = null;
                    if (reader.Read())
                    {
                        vehicle = VehicleMapper.FromReader(reader);
                    }
                    return vehicle;
                },
                options: new QueryOptions(),
                tableAlias: "v",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }

        #endregion


    }
}
