using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class ProductRepository : Repository<ProductRepository>, IProductRepository
    {
        public ProductRepository(string connectionString, ILogger<ProductRepository> logger) : base(connectionString, logger)
        {
        }

        public Product Add(Product product)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO Products 
                    (Name, BarCode, UnitType, Price, Description, TemperatureCondition, Stock, Image, Observations, SubCategoryId)
                    OUTPUT INSERTED.Id 
                    VALUES 
                    (@Name, @BarCode, @UnitType, @Price, @Description, @TemperatureCondition, @Stock, @Image, @Observations, @SubCategoryId)", connection);

                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@BarCode", product.Barcode);
                command.Parameters.AddWithValue("@UnitType", product.UnitType.ToString());
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@TemperatureCondition", product.TemperatureCondition.ToString());
                command.Parameters.AddWithValue("@Stock", product.Stock);
                command.Parameters.AddWithValue("@Image", product.Image);
                command.Parameters.AddWithValue("@Observations", product.Observation);
                command.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);

                connection.Open();
                int id = (int)command.ExecuteScalar();

                return new Product(id, product.Barcode, product.Name, product.Price, product.Image, product.Stock,
                    product.Description, product.UnitType, product.TemperatureCondition, product.Observation,
                    product.SubCategory, new());
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

        public Product? Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Intentando eliminar el producto con ID {id}.");
                using var connection = CreateConnection();
                connection.Open();

                var product = GetById(id);
                if (product == null)
                {
                    _logger.LogWarning($"No se encontró el producto con ID {id} para eliminar.");
                    return null;
                }

                using (var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                _logger.LogInformation($"Producto con ID {id} eliminado correctamente.");
                return product;
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

        public List<Product> GetAll()
        {
            try
            {
                var products = new List<Product>();

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand(@"
                    SELECT 
                        p.*, 
                        sc.Name AS SubCategoryName, 
                        sc.Description AS SubCategoryDescription, 
                        c.Id AS CategoryId, 
                        c.Name AS CategoryName, 
                        c.Description AS CategoryDescription
                    FROM Products p
                    JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                    JOIN Categories c ON sc.CategoryId = c.Id", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(ProductMapper.FromReader(reader));
                    }
                }

                return products;
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

        public Product GetByBarcode(string barcode)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand(@"
                    SELECT 
                        p.*, 
                        sc.Name AS SubCategoryName, 
                        sc.Description AS SubCategoryDescription, 
                        c.Id AS CategoryId, 
                        c.Name AS CategoryName, 
                        c.Description AS CategoryDescription
                    FROM Products p
                    JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                    JOIN Categories c ON sc.CategoryId = c.Id
                    WHERE p.Barcode = @Barcode", connection))
                {
                    command.Parameters.AddWithValue("@Barcode", barcode);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    return ProductMapper.FromReader(reader);
                }
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

        public Product? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand(@"
                    SELECT 
                        p.*, 
                        sc.Name AS SubCategoryName, 
                        sc.Description AS SubCategoryDescription, 
                        c.Id AS CategoryId, 
                        c.Name AS CategoryName, 
                        c.Description AS CategoryDescription
                    FROM Products p
                    JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                    JOIN Categories c ON sc.CategoryId = c.Id
                    WHERE p.Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    return ProductMapper.FromReader(reader);
                }
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

        public Product GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand(@"
                    SELECT 
                        p.*, 
                        sc.Name AS SubCategoryName, 
                        sc.Description AS SubCategoryDescription, 
                        c.Id AS CategoryId, 
                        c.Name AS CategoryName, 
                        c.Description AS CategoryDescription
                    FROM Products p
                    JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                    JOIN Categories c ON sc.CategoryId = c.Id
                    WHERE p.Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    return ProductMapper.FromReader(reader);
                }
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

        public Product Update(Product product)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var updateCommand = new SqlCommand(@"
                    UPDATE Products SET 
                        Name = @Name,
                        BarCode = @BarCode,
                        UnitType = @UnitType,
                        Price = @Price,
                        Description = @Description,
                        TemperatureCondition = @TemperatureCondition,
                        Stock = @Stock,
                        Image = @Image,
                        Observations = @Observations,
                        SubCategoryId = @SubCategoryId
                    WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", product.Name);
                    updateCommand.Parameters.AddWithValue("@BarCode", product.Barcode);
                    updateCommand.Parameters.AddWithValue("@UnitType", product.UnitType.ToString());
                    updateCommand.Parameters.AddWithValue("@Price", product.Price);
                    updateCommand.Parameters.AddWithValue("@Description", product.Description);
                    updateCommand.Parameters.AddWithValue("@TemperatureCondition", product.TemperatureCondition.ToString());
                    updateCommand.Parameters.AddWithValue("@Stock", product.Stock);
                    updateCommand.Parameters.AddWithValue("@Image", product.Image);
                    updateCommand.Parameters.AddWithValue("@Observations", product.Observation);
                    updateCommand.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    updateCommand.Parameters.AddWithValue("@Id", product.Id);

                    int affectedRows = updateCommand.ExecuteNonQuery();
                    if (affectedRows == 0)
                        throw new InvalidOperationException($"No se encontró el producto con ID {product.Id} para actualizar.");
                }

                return product;
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
