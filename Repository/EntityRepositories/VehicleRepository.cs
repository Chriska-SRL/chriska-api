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
                using var command = new SqlCommand(@"
                    INSERT INTO Vehicles (Plate, CrateCapacity, Brand, Model)
                    OUTPUT INSERTED.Id
                    VALUES (@Plate, @CrateCapacity, @Brand, @Model)", connection);

                command.Parameters.AddWithValue("@Plate", vehicle.Plate);
                command.Parameters.AddWithValue("@CrateCapacity", vehicle.CrateCapacity);
                command.Parameters.AddWithValue("@Brand", vehicle.Brand);
                command.Parameters.AddWithValue("@Model", vehicle.Model);

                connection.Open();
                int id = (int)command.ExecuteScalar();

                return new Vehicle(id, vehicle.Plate, vehicle.Brand, vehicle.Model, vehicle.CrateCapacity, new List<VehicleCost>());
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

                var vehicle = GetById(id);
                if (vehicle == null)
                {
                    _logger.LogWarning($"No se encontró el vehículo con ID {id} para eliminar.");
                    return null;
                }

                using var connection = CreateConnection();
                using var command = new SqlCommand("DELETE FROM Vehicles WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();

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
                using var command = new SqlCommand("SELECT Id, Plate, CrateCapacity, Brand, Model FROM Vehicles", connection);

                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    vehicles.Add(VehicleMapper.FromReader(reader));
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
                using var command = new SqlCommand("SELECT Id, Plate, CrateCapacity, Brand, Model FROM Vehicles WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using var reader = command.ExecuteReader();
                return reader.Read() ? VehicleMapper.FromReader(reader) : null;
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

        public Vehicle? GetByPlate(string plate)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand("SELECT Id, Plate, CrateCapacity, Brand, Model FROM Vehicles WHERE Plate = @Plate", connection);

                command.Parameters.AddWithValue("@Plate", plate);
                connection.Open();

                using var reader = command.ExecuteReader();
                return reader.Read() ? VehicleMapper.FromReader(reader) : null;
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
                using var command = new SqlCommand(@"
                    UPDATE Vehicles 
                    SET Plate = @Plate, CrateCapacity = @CrateCapacity, Brand = @Brand, Model = @Model 
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Plate", vehicle.Plate);
                command.Parameters.AddWithValue("@CrateCapacity", vehicle.CrateCapacity);
                command.Parameters.AddWithValue("@Brand", vehicle.Brand);
                command.Parameters.AddWithValue("@Model", vehicle.Model);
                command.Parameters.AddWithValue("@Id", vehicle.Id);

                connection.Open();
                int updated = command.ExecuteNonQuery();
                if (updated == 0)
                    throw new InvalidOperationException($"No se encontró el vehículo con ID {vehicle.Id} para actualizar.");

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
    }
}
