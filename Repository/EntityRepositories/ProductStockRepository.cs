using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ProductStockRepository : Repository<ProductStockRepository>, IProductStockRepository
    {
        public ProductStockRepository(string connectionString, ILogger<ProductStockRepository> logger) : base(connectionString, logger)
        {
        }

        public ProductStock Add(ProductStock stock)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO ProductsStock (ProductId, ShelveId, Quantity)
                    OUTPUT INSERTED.Id
                    VALUES (@ProductId, @ShelveId, @Quantity)", connection);

                command.Parameters.AddWithValue("@ProductId", stock.Product.Id);
                command.Parameters.AddWithValue("@ShelveId", stock.Shelve.Id);
                command.Parameters.AddWithValue("@Quantity", stock.Quantity);

                connection.Open();
                int newId = (int)command.ExecuteScalar();

                return new ProductStock(newId, stock.Quantity, stock.Product, stock.Shelve);
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

        public ProductStock? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var stock = GetByIdInternal(id, connection);
                if (stock == null)
                    return null;

                using var command = new SqlCommand("DELETE FROM ProductsStock WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                int deleted = command.ExecuteNonQuery();
                if (deleted == 0)
                    throw new InvalidOperationException($"No se eliminó el registro de stock con ID {id}.");

                return stock;
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

        public List<ProductStock> GetAll()
        {
            try
            {
                var result = new List<ProductStock>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT Id, Quantity, ProductId, ShelveId FROM ProductsStock", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(ProductStockMapper.FromReader(reader));
                }

                return result;
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

        public ProductStock? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                return GetByIdInternal(id, connection);
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

        public ProductStock Update(ProductStock stock)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var update = new SqlCommand(@"
                    UPDATE ProductsStock 
                    SET Quantity = @Quantity, ProductId = @ProductId, ShelveId = @ShelveId
                    WHERE Id = @Id", connection);

                update.Parameters.AddWithValue("@Quantity", stock.Quantity);
                update.Parameters.AddWithValue("@ProductId", stock.Product.Id);
                update.Parameters.AddWithValue("@ShelveId", stock.Shelve.Id);
                update.Parameters.AddWithValue("@Id", stock.Id);

                int affected = update.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException($"No se encontró el registro con ID {stock.Id} para actualizar.");

                return stock;
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

        private ProductStock? GetByIdInternal(int id, SqlConnection connection)
        {
            using var command = new SqlCommand("SELECT Id, Quantity, ProductId, ShelveId FROM ProductsStock WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            return reader.Read() ? ProductStockMapper.FromReader(reader) : null;
        }
    }
}
