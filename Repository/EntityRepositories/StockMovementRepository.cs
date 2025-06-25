using BusinessLogic.Común.Enums;
using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class StockMovementRepository : Repository<StockMovementRepository>, IStockMovementRepository
    {
        public StockMovementRepository(string connectionString, ILogger<StockMovementRepository> logger) : base(connectionString, logger)
        {
        }
        const string BaseQuery = @"
            SELECT 
                sm.Id, sm.Quantity, sm.Type, sm.Reason, sm.Date,
                p.Id AS ProductId, p.Name AS ProductName, p.BarCode, p.UnitType, p.Price, 
                p.Description AS ProductDescription, p.TemperatureCondition, p.Stock AS ProductStock, 
                p.Image, p.Observations,
                sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription,
                c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription,
                s.Id AS ShelveId, s.Name AS ShelveName, s.Description AS ShelveDescription,
                w.Id AS WarehouseId, w.Name AS WarehouseName, w.Description AS WarehouseDescription, w.Address,
                u.Id AS UserId, u.Name AS UserName, u.Username, u.Password, u.IsEnabled, u.NeedsPasswordChange,
                r.Id AS RoleId, r.Name AS RoleName, r.Description AS RoleDescription
            FROM StockMovements sm
            JOIN Products p ON sm.ProductId = p.Id
            JOIN SubCategories sc ON p.SubCategoryId = sc.Id
            JOIN Categories c ON sc.CategoryId = c.Id
            JOIN Shelves s ON sm.ShelveId = s.Id
            JOIN Warehouses w ON s.WarehouseId = w.Id
            JOIN Users u ON sm.UserId = u.Id
            JOIN Roles r ON u.RoleId = r.Id
            ";

        public StockMovement Add(StockMovement movement)
        {
            try
            {
                int newId;

                using (var connection = CreateConnection())
                {
                    using var command = new SqlCommand(@"
                DECLARE @output TABLE (Id INT);

                INSERT INTO StockMovements (Quantity, Type, Reason, Date, ProductId, UserId, ShelveId)
                OUTPUT INSERTED.Id INTO @output
                VALUES (@Quantity, @Type, @Reason, @Date, @ProductId, @UserId, @ShelveId);

                SELECT Id FROM @output;", connection);

                    command.Parameters.AddWithValue("@Quantity", movement.Quantity);
                    command.Parameters.AddWithValue("@Type", movement.Type == StockMovementType.Ingreso ? "I" : "E");
                    command.Parameters.AddWithValue("@Reason", movement.Reason);
                    command.Parameters.AddWithValue("@Date", movement.Date);
                    command.Parameters.AddWithValue("@ProductId", movement.Product.Id);
                    command.Parameters.AddWithValue("@UserId", movement.User.Id);
                    command.Parameters.AddWithValue("@ShelveId", movement.Shelve.Id);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        throw new Exception("No se pudo obtener el ID del nuevo movimiento.");

                    newId = Convert.ToInt32(result);
                }

                using var refreshConnection = CreateConnection();
                refreshConnection.Open();
                return GetByIdInternal(newId, refreshConnection)!;
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
        public StockMovement? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var movement = GetByIdInternal(id, connection);
                if (movement == null)
                    return null;

                using var deleteCmd = new SqlCommand("DELETE FROM StockMovements WHERE Id = @Id", connection);
                deleteCmd.Parameters.AddWithValue("@Id", id);

                int deleted = deleteCmd.ExecuteNonQuery();
                if (deleted == 0)
                    throw new InvalidOperationException($"No se eliminó el movimiento con ID {id}.");

                return movement;
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

        public List<StockMovement> GetAll()
        {
            try
            {
                var result = new List<StockMovement>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(BaseQuery, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(StockMovementMapper.FromReader(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los movimientos.");
                throw new ApplicationException("Error al obtener todos los movimientos.", ex);
            }
        }


        private StockMovement? GetByIdInternal(int id, SqlConnection connection)
        {
            using var command = new SqlCommand(BaseQuery + " WHERE sm.Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            return reader.Read() ? StockMovementMapper.FromReader(reader) : null;
        }

        public StockMovement? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                return GetByIdInternal(id, connection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimiento por ID.");
                throw new ApplicationException("Error al obtener el movimiento.", ex);
            }
        }


        public StockMovement Update(StockMovement movement)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var update = new SqlCommand(@"
                    UPDATE StockMovements 
                    SET Quantity = @Quantity, Type = @Type, Reason = @Reason, Date = @Date, 
                        ProductId = @ProductId, UserId = @UserId, ShelveId = @ShelveId 
                    WHERE Id = @Id", connection);

                update.Parameters.AddWithValue("@Quantity", movement.Quantity);
                update.Parameters.AddWithValue("@Type", movement.Type);
                update.Parameters.AddWithValue("@Reason", movement.Reason);
                update.Parameters.AddWithValue("@Date", movement.Date);
                update.Parameters.AddWithValue("@ProductId", movement.Product.Id);
                update.Parameters.AddWithValue("@UserId", movement.User.Id);
                update.Parameters.AddWithValue("@ShelveId", movement.Shelve.Id);
                update.Parameters.AddWithValue("@Id", movement.Id);

                int affected = update.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException($"No se encontró el movimiento con ID {movement.Id} para actualizar.");

                return movement;
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


        public List<StockMovement> GetAll(DateTime from, DateTime to)
        {
            try
            {
                var result = new List<StockMovement>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(BaseQuery + " WHERE sm.Date BETWEEN @From AND @To", connection);
                command.Parameters.AddWithValue("@From", from);
                command.Parameters.AddWithValue("@To", to);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(StockMovementMapper.FromReader(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos por rango.");
                throw new ApplicationException("Error al obtener los movimientos.", ex);
            }
        }


        public List<StockMovement> GetAllByShelve(int id, DateTime from, DateTime to)
        {
            try
            {
                var result = new List<StockMovement>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(BaseQuery + " WHERE sm.ShelveId = @ShelveId AND sm.Date BETWEEN @From AND @To", connection);
                command.Parameters.AddWithValue("@ShelveId", id);
                command.Parameters.AddWithValue("@From", from);
                command.Parameters.AddWithValue("@To", to);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(StockMovementMapper.FromReader(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos por estantería.");
                throw new ApplicationException("Error al obtener los movimientos por estantería.", ex);
            }
        }


        public List<StockMovement> GetAllByWarehouse(int id, DateTime from, DateTime to)
        {
            try
            {
                var result = new List<StockMovement>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(BaseQuery + " WHERE w.Id = @WarehouseId AND sm.Date BETWEEN @From AND @To", connection);
                command.Parameters.AddWithValue("@WarehouseId", id);
                command.Parameters.AddWithValue("@From", from);
                command.Parameters.AddWithValue("@To", to);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(StockMovementMapper.FromReader(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos por depósito.");
                throw new ApplicationException("Error al obtener los movimientos por depósito.", ex);
            }
        }



    }
}
