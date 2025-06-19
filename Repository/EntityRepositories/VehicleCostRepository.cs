using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class VehicleCostRepository : Repository<VehicleCost>, IVehicleCostRepository
    {
        public VehicleCostRepository(string connectionString, ILogger<VehicleCost> logger) : base(connectionString, logger)
        {
        }

        public VehicleCost Add(VehicleCost cost)
        {
            try
            {
                cost.Validate();

                using var connection = CreateConnection();
                connection.Open();

                var command = new SqlCommand(@"
                    INSERT INTO VehicleCosts (VehicleId, Type, Description, Amount, Date)
                    OUTPUT INSERTED.Id
                    VALUES (@VehicleId, @Type, @Description, @Amount, @Date)", connection);

                command.Parameters.AddWithValue("@VehicleId", cost.VehicleId);
                command.Parameters.AddWithValue("@Type", (int)cost.Type);
                command.Parameters.AddWithValue("@Description", cost.Description);
                command.Parameters.AddWithValue("@Amount", cost.Amount);
                command.Parameters.AddWithValue("@Date", cost.Date);

                int id = (int)command.ExecuteScalar();
                return new VehicleCost(id, cost.VehicleId, cost.Type, cost.Description, cost.Amount, cost.Date);
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

        public VehicleCost? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var cost = GetById(id);
                if (cost == null)
                    return null;

                var command = new SqlCommand("DELETE FROM VehicleCosts WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
                return cost;
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

        public List<VehicleCost> GetAll()
        {
            throw new NotImplementedException("Este método no estara implementado en VehicleCostRepository debido a su volumen potencial. Use GetAllForVehicle(int vehicleId) en su lugar.");
        }

        public List<VehicleCost> GetAllForVehicle(int vehicleId)
        {
            try
            {
                var list = new List<VehicleCost>();

                using var connection = CreateConnection();
                connection.Open();

                var command = new SqlCommand("SELECT Id, VehicleId, Type, Description, Amount, Date FROM VehicleCosts WHERE VehicleId = @VehicleId", connection);
                command.Parameters.AddWithValue("@VehicleId", vehicleId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(VehicleCostMapper.FromReader(reader));
                }

                return list;
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

        public VehicleCost? GetById(int costId)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var command = new SqlCommand(@"
            SELECT Id, VehicleId, Type, Description, Amount, Date 
            FROM VehicleCosts 
            WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", costId);

                using var reader = command.ExecuteReader();
                return reader.Read() ? VehicleCostMapper.FromReader(reader) : null;
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

        public List<VehicleCost> GetCostsForVehicleInDateRange(int vehicleId, DateTime from, DateTime to)
        {
            try
            {
                var list = new List<VehicleCost>();

                using var connection = CreateConnection();
                connection.Open();

                var command = new SqlCommand(@"
            SELECT Id, VehicleId, Type, Description, Amount, Date
            FROM VehicleCosts
            WHERE VehicleId = @VehicleId AND Date BETWEEN @From AND @To", connection);

                command.Parameters.AddWithValue("@VehicleId", vehicleId);
                command.Parameters.AddWithValue("@From", from);
                command.Parameters.AddWithValue("@To", to);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(VehicleCostMapper.FromReader(reader));
                }

                return list;
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
        public List<VehicleCost> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var costs = new List<VehicleCost>();

                using var connection = CreateConnection();
                connection.Open();

                var command = new SqlCommand(@"
            SELECT Id, VehicleId, Type, Description, Amount, Date 
            FROM VehicleCosts 
            WHERE Date BETWEEN @StartDate AND @EndDate
            ORDER BY Date ASC", connection);

                command.Parameters.AddWithValue("@StartDate", startDate.Date);
                command.Parameters.AddWithValue("@EndDate", endDate.Date);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    costs.Add(VehicleCostMapper.FromReader(reader));
                }

                return costs;
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


        public VehicleCost Update(VehicleCost cost)
        {
            try
            {
                cost.Validate();

                using var connection = CreateConnection();
                connection.Open();

                var command = new SqlCommand(@"
                    UPDATE VehicleCosts
                    SET Type = @Type, Description = @Description, Amount = @Amount, Date = @Date
                    WHERE VehicleId = @VehicleId AND Id = @Id", connection);

                command.Parameters.AddWithValue("@VehicleId", cost.VehicleId);
                command.Parameters.AddWithValue("@Id", cost.Id);
                command.Parameters.AddWithValue("@Type", (int)cost.Type);
                command.Parameters.AddWithValue("@Description", cost.Description);
                command.Parameters.AddWithValue("@Amount", cost.Amount);
                command.Parameters.AddWithValue("@Date", cost.Date);

                int affected = command.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException("No se encontró el costo para actualizar.");

                return cost;
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
