using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ShelveRepository : Repository<ShelveRepository>, IShelveRepository
    {
        public ShelveRepository(string connectionString, ILogger<ShelveRepository> logger)
            : base(connectionString, logger) { }

        public Shelve Add(Shelve shelve)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    INSERT INTO Shelves (Name, Description, WarehouseId) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Description, @WarehouseId)", connection);

                command.Parameters.AddWithValue("@Name", shelve.Name);
                command.Parameters.AddWithValue("@Description", shelve.Description);
                command.Parameters.AddWithValue("@WarehouseId", shelve.Warehouse.Id);

                int newId = (int)command.ExecuteScalar();
                var warehouse = GetWarehouseById(shelve.Warehouse.Id, connection);

                return new Shelve(newId, shelve.Name, shelve.Description, warehouse, new(), new());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar estante.");
                throw new ApplicationException("Error al agregar estante.", ex);
            }
        }

        public Shelve? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var shelve = GetBasicById(id, connection);
                if (shelve == null) return null;

                using var command = new SqlCommand("DELETE FROM Shelves WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                if (command.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"No se eliminó el estante con ID {id}.");

                return shelve;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar estante.");
                throw new ApplicationException("Error al eliminar estante.", ex);
            }
        }

        public List<Shelve> GetAll()
        {
            try
            {
                var result = new List<Shelve>();
                using var connection = CreateConnection();
                connection.Open();

                var query = @"
                    SELECT 
                        s.Id AS Shelve_Id, s.Name AS Shelve_Name, s.Description AS Shelve_Description, s.WarehouseId AS Shelve_WarehouseId,
                        w.Id AS Warehouse_Id, w.Name AS Warehouse_Name, w.Description AS Warehouse_Description, w.Address AS Warehouse_Address
                    FROM Shelves s
                    INNER JOIN Warehouses w ON s.WarehouseId = w.Id";

                using var command = new SqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var warehouse = WarehouseMapper.FromReader(reader, "Warehouse_");
                    var shelve = ShelveMapper.FromReader(reader, "Shelve_");
                    shelve.Warehouse = warehouse;
                    result.Add(shelve);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estantes.");
                throw new ApplicationException("Error al obtener estantes.", ex);
            }
        }

        public Shelve? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var shelve = GetBasicById(id, connection);
                if (shelve == null) return null;

                shelve.Stocks = GetProductStocksByShelveId(shelve.Id, connection);

                return shelve;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estante por ID.");
                throw new ApplicationException("Error al obtener estante por ID.", ex);
            }
        }

        public Shelve? GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                Shelve? shelve = null;
                using (var command = new SqlCommand(@"
                    SELECT Id, Name, Description, WarehouseId 
                    FROM Shelves 
                    WHERE Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                        shelve = ShelveMapper.FromReader(reader);
                }

                if (shelve != null)
                {
                    shelve.Warehouse = GetWarehouseById(shelve.Warehouse.Id, connection);
                }

                return shelve;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estante por nombre.");
                throw new ApplicationException("Error al obtener estante por nombre.", ex);
            }
        }

        public Shelve Update(Shelve shelve)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Shelves 
                    SET Name = @Name, Description = @Description, WarehouseId = @WarehouseId 
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Name", shelve.Name);
                command.Parameters.AddWithValue("@Description", shelve.Description);
                command.Parameters.AddWithValue("@WarehouseId", shelve.Warehouse.Id);
                command.Parameters.AddWithValue("@Id", shelve.Id);

                if (command.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException($"No se encontró el estante con ID {shelve.Id}.");

                var warehouse = GetWarehouseById(shelve.Warehouse.Id, connection);
                return new Shelve(shelve.Id, shelve.Name, shelve.Description, warehouse, new(), new());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estante.");
                throw new ApplicationException("Error al actualizar estante.", ex);
            }
        }

        private Shelve? GetBasicById(int id, SqlConnection connection)
        {
            var query = @"
                SELECT 
                    s.Id AS Shelve_Id, s.Name AS Shelve_Name, s.Description AS Shelve_Description, s.WarehouseId AS Shelve_WarehouseId,
                    w.Id AS Warehouse_Id, w.Name AS Warehouse_Name, w.Description AS Warehouse_Description, w.Address AS Warehouse_Address
                FROM Shelves s
                INNER JOIN Warehouses w ON s.WarehouseId = w.Id
                WHERE s.Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            var warehouse = WarehouseMapper.FromReader(reader, "Warehouse_");
            var shelve = ShelveMapper.FromReader(reader, "Shelve_");
            shelve.Warehouse = warehouse;
            return shelve;
        }

        private Warehouse GetWarehouseById(int id, SqlConnection connection)
        {
            using var command = new SqlCommand("SELECT Id, Name, Description, Address FROM Warehouses WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            if (!reader.Read()) throw new Exception($"No se encontró el depósito con ID {id}.");
            return WarehouseMapper.FromReader(reader);
        }

        private List<ProductStock> GetProductStocksByShelveId(int shelveId, SqlConnection connection)
        {
            var result = new List<ProductStock>();
            var command = new SqlCommand("SELECT Id, ProductId, Quantity, ShelveId FROM ProductsStock WHERE ShelveId = @ShelveId", connection);
            command.Parameters.AddWithValue("@ShelveId", shelveId);

            var stocks = new List<ProductStock>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    stocks.Add(ProductStockMapper.FromReader(reader));
                }
            }

            foreach (var stock in stocks)
            {
                stock.Product = GetProductById(stock.Product.Id, connection);
                result.Add(stock);
            }

            return result;
        }


        private Product GetProductById(int id, SqlConnection connection)
        {
            var command = new SqlCommand(@"
                SELECT 
                    p.Id, p.Name, p.BarCode, p.UnitType, p.Price, p.Description,
                    p.TemperatureCondition, p.Stock, p.Image, p.Observations, 
                    p.SubCategoryId,
                    sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription,
                    c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription
                FROM Products p
                INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                INNER JOIN Categories c ON sc.CategoryId = c.Id
                WHERE p.Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) throw new Exception($"No se encontró el producto con ID {id}.");
            return ProductMapper.FromReader(reader);
        }
    }
}
