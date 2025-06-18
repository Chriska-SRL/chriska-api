using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(string connectionString, ILogger<Vehicle> logger) : base(connectionString, logger)
        {
        }

        public Vehicle Add(Vehicle vehicle)
        {
            try
            {
                vehicle.Validate();

                using var connection = CreateConnection();
                connection.Open();

                using var transaction = connection.BeginTransaction();

                var insertVehicle = new SqlCommand(@"
                    INSERT INTO Vehicles (Plate, CrateCapacity, Brand, Model, LastCostId)
                    OUTPUT INSERTED.Id
                    VALUES (@Plate, @CrateCapacity, @Brand, @Model, @LastCostId)", connection, transaction);

                insertVehicle.Parameters.AddWithValue("@Plate", vehicle.Plate);
                insertVehicle.Parameters.AddWithValue("@CrateCapacity", vehicle.CrateCapacity);
                insertVehicle.Parameters.AddWithValue("@Brand", vehicle.Brand);
                insertVehicle.Parameters.AddWithValue("@Model", vehicle.Model);
                insertVehicle.Parameters.AddWithValue("@LastCostId", vehicle.GetLastCostId());

                int vehicleId = (int)insertVehicle.ExecuteScalar();
                vehicle = new Vehicle(vehicleId, vehicle.Plate, vehicle.Brand, vehicle.Model, vehicle.CrateCapacity, vehicle.GetLastCostId(), vehicle.VehicleCosts);

                foreach (var cost in vehicle.VehicleCosts)
                {
                    var insertCost = new SqlCommand(@"
                        INSERT INTO VehicleCosts (VehicleId, Id, Type, Amount, Description)
                        VALUES (@VehicleId, @Id, @Type, @Amount, @Description)", connection, transaction);

                    insertCost.Parameters.AddWithValue("@VehicleId", vehicleId);
                    insertCost.Parameters.AddWithValue("@Id", cost.Id);
                    insertCost.Parameters.AddWithValue("@Type", (int)cost.Type);
                    insertCost.Parameters.AddWithValue("@Amount", cost.Amount);
                    insertCost.Parameters.AddWithValue("@Description", cost.Description);
                    insertCost.ExecuteNonQuery();
                }

                transaction.Commit();
                return vehicle;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Vehicle? Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Intentando eliminar el vehículo con ID {id}.");
                using var connection = CreateConnection();
                connection.Open();

                var vehicle = GetVehicleWithoutCosts(id, connection);
                if (vehicle == null)
                {
                    _logger.LogWarning($"No se encontró el vehículo con ID {id} para eliminar.");
                    return null;
                }

                vehicle.SetCosts(GetCostsForVehicle(id, connection));

                using (var deleteCosts = new SqlCommand("DELETE FROM VehicleCosts WHERE VehicleId = @Id", connection))
                {
                    deleteCosts.Parameters.AddWithValue("@Id", id);
                    deleteCosts.ExecuteNonQuery();
                }

                using (var deleteVehicle = new SqlCommand("DELETE FROM Vehicles WHERE Id = @Id", connection))
                {
                    deleteVehicle.Parameters.AddWithValue("@Id", id);
                    int deleted = deleteVehicle.ExecuteNonQuery();
                    if (deleted == 0)
                        throw new InvalidOperationException($"El vehículo con ID {id} no fue eliminado.");
                }

                _logger.LogInformation($"Vehículo con ID {id} eliminado correctamente.");
                return vehicle;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public List<Vehicle> GetAll()
        {
            try
            {
                var vehicles = new List<Vehicle>();

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Plate, Brand, Model, CrateCapacity, LastCostId FROM Vehicles", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vehicles.Add(VehicleMapper.FromReader(reader));
                    }
                }

                foreach (var vehicle in vehicles)
                {
                    vehicle.SetCosts(GetCostsForVehicle(vehicle.Id, connection));
                }

                return vehicles;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Vehicle? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var vehicle = GetVehicleWithoutCosts(id, connection);
                if (vehicle == null) return null;

                vehicle.SetCosts(GetCostsForVehicle(id, connection));
                return vehicle;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Vehicle GetByPlate(string plate)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var vehicle = GetVehicleWithoutCosts(plate, connection);
                if (vehicle == null) return null;

                vehicle.SetCosts(GetCostsForVehicle(vehicle.Id, connection));
                return vehicle;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Vehicle Update(Vehicle vehicle)
        {
            try
            {
                vehicle.Validate();

                using var connection = CreateConnection();
                connection.Open();

                using var transaction = connection.BeginTransaction();

                var updateCommand = new SqlCommand(@"
                    UPDATE Vehicles 
                    SET Plate = @Plate, CrateCapacity = @CrateCapacity, Brand = @Brand, Model = @Model, LastCostId = @LastCostId 
                    WHERE Id = @Id", connection, transaction);

                updateCommand.Parameters.AddWithValue("@Plate", vehicle.Plate);
                updateCommand.Parameters.AddWithValue("@CrateCapacity", vehicle.CrateCapacity);
                updateCommand.Parameters.AddWithValue("@Brand", vehicle.Brand);
                updateCommand.Parameters.AddWithValue("@Model", vehicle.Model);
                updateCommand.Parameters.AddWithValue("@LastCostId", vehicle.GetLastCostId());
                updateCommand.Parameters.AddWithValue("@Id", vehicle.Id);

                int rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected == 0)
                    throw new InvalidOperationException($"No se encontró el vehículo con ID {vehicle.Id} para actualizar.");

                var deleteCosts = new SqlCommand("DELETE FROM VehicleCosts WHERE VehicleId = @VehicleId", connection, transaction);
                deleteCosts.Parameters.AddWithValue("@VehicleId", vehicle.Id);
                deleteCosts.ExecuteNonQuery();

                foreach (var cost in vehicle.VehicleCosts)
                {
                    var insertCost = new SqlCommand(@"
                        INSERT INTO VehicleCosts (VehicleId, Id, Type, Amount, Description)
                        VALUES (@VehicleId, @Id, @Type, @Amount, @Description)", connection, transaction);

                    insertCost.Parameters.AddWithValue("@VehicleId", vehicle.Id);
                    insertCost.Parameters.AddWithValue("@Id", cost.Id);
                    insertCost.Parameters.AddWithValue("@Type", (int)cost.Type);
                    insertCost.Parameters.AddWithValue("@Amount", cost.Amount);
                    insertCost.Parameters.AddWithValue("@Description", cost.Description);
                    insertCost.ExecuteNonQuery();
                }

                transaction.Commit();
                return vehicle;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        private Vehicle? GetVehicleWithoutCosts(int id, SqlConnection connection)
        {
            using var command = new SqlCommand("SELECT Id, Plate, Brand, Model, CrateCapacity, LastCostId FROM Vehicles WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? VehicleMapper.FromReader(reader) : null;
        }

        private Vehicle? GetVehicleWithoutCosts(string plate, SqlConnection connection)
        {
            using var command = new SqlCommand("SELECT Id, Plate, Brand, Model, CrateCapacity, LastCostId FROM Vehicles WHERE Plate = @Plate", connection);
            command.Parameters.AddWithValue("@Plate", plate);
            using var reader = command.ExecuteReader();
            return reader.Read() ? VehicleMapper.FromReader(reader) : null;
        }

        private List<VehicleCost> GetCostsForVehicle(int vehicleId, SqlConnection connection)
        {
            var costs = new List<VehicleCost>();
            using var command = new SqlCommand("SELECT Id, Type, Description, Amount FROM VehicleCosts WHERE VehicleId = @VehicleId", connection);
            command.Parameters.AddWithValue("@VehicleId", vehicleId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                costs.Add(VehicleCostMapper.FromReader(reader));
            }
            return costs;
        }
    }
}
