using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class WarehouseRepository : Repository<WarehouseRepository>, IWarehouseRepository
    {
        public WarehouseRepository(string connectionString, ILogger<WarehouseRepository> logger)
            : base(connectionString, logger) { }

        public Warehouse Add(Warehouse warehouse)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO Warehouses (Name, Description, Address) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Description, @Address)", connection);

                command.Parameters.AddWithValue("@Name", warehouse.Name);
                command.Parameters.AddWithValue("@Description", warehouse.Description);
                command.Parameters.AddWithValue("@Address", warehouse.Address);

                connection.Open();
                int newId = (int)command.ExecuteScalar();

                return new Warehouse(newId, warehouse.Name, warehouse.Description, warehouse.Address, new List<Shelve>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar almacén.");
                throw new ApplicationException("Error al agregar almacén.", ex);
            }
        }

        public Warehouse? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var warehouse = GetByIdInternal(id, connection);
                if (warehouse == null) return null;

                using var deleteCmd = new SqlCommand("DELETE FROM Warehouses WHERE Id = @Id", connection);
                deleteCmd.Parameters.AddWithValue("@Id", id);

                int deleted = deleteCmd.ExecuteNonQuery();
                if (deleted == 0)
                    throw new InvalidOperationException($"No se eliminó el almacén con ID {id}.");

                return warehouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar almacén.");
                throw new ApplicationException("Error al eliminar almacén.", ex);
            }
        }

        public List<Warehouse> GetAll()
        {
            try
            {
                var result = new List<Warehouse>();

                using var connection = CreateConnection();
                connection.Open();

                var warehouses = new List<Warehouse>();
                using (var command = new SqlCommand("SELECT Id, Name, Description, Address FROM Warehouses", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        warehouses.Add(WarehouseMapper.FromReader(reader));
                    }
                } // reader cerrado automáticamente

                foreach (var warehouse in warehouses)
                {
                    warehouse.Shelves = GetShelvesByWarehouseId(warehouse.Id, connection);
                    result.Add(warehouse);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener almacenes.");
                throw new ApplicationException("Error al obtener almacenes.", ex);
            }
        }

        public Warehouse? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                return GetByIdInternal(id, connection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener almacén por ID.");
                throw new ApplicationException("Error al obtener almacén por ID.", ex);
            }
        }

        public Warehouse? GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                Warehouse? warehouse;
                using (var command = new SqlCommand("SELECT Id, Name, Description, Address FROM Warehouses WHERE Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;
                    warehouse = WarehouseMapper.FromReader(reader);
                }

                warehouse.Shelves = GetShelvesByWarehouseId(warehouse.Id, connection);
                return warehouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener almacén por nombre.");
                throw new ApplicationException("Error al obtener almacén por nombre.", ex);
            }
        }

        public Warehouse Update(Warehouse warehouse)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Warehouses 
                    SET Name = @Name, Description = @Description, Address = @Address 
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Name", warehouse.Name);
                command.Parameters.AddWithValue("@Description", warehouse.Description);
                command.Parameters.AddWithValue("@Address", warehouse.Address);
                command.Parameters.AddWithValue("@Id", warehouse.Id);

                int affected = command.ExecuteNonQuery();
                if (affected == 0)
                    throw new InvalidOperationException($"No se encontró el almacén con ID {warehouse.Id}.");

                return warehouse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar almacén.");
                throw new ApplicationException("Error al actualizar almacén.", ex);
            }
        }

        private Warehouse? GetByIdInternal(int id, SqlConnection connection)
        {
            Warehouse? warehouse;

            using (var command = new SqlCommand("SELECT Id, Name, Description, Address FROM Warehouses WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                warehouse = WarehouseMapper.FromReader(reader);
            }

            warehouse.Shelves = GetShelvesByWarehouseId(id, connection);
            return warehouse;
        }

        private List<Shelve> GetShelvesByWarehouseId(int warehouseId, SqlConnection connection)
        {
            var result = new List<Shelve>();

            var query = @"
                        SELECT 
                            s.Id AS ShelveId, s.Name AS ShelveName, s.Description AS ShelveDescription,
                            w.Id AS WarehouseId, w.Name AS WarehouseName, w.Description AS WarehouseDescription, w.Address AS WarehouseAddress
                        FROM Shelves s
                        INNER JOIN Warehouses w ON s.WarehouseId = w.Id
                        WHERE s.WarehouseId = @WarehouseId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var warehouse = new Warehouse
                    (
                        id: reader.GetInt32(reader.GetOrdinal("WarehouseId")),
                        name: reader.GetString(reader.GetOrdinal("WarehouseName")),
                        description: reader.GetString(reader.GetOrdinal("WarehouseDescription")),
                        address: reader.GetString(reader.GetOrdinal("WarehouseAddress")),
                        shelves: new List<Shelve>()
                    );

                    var shelve = new Shelve
                    (
                        id: reader.GetInt32(reader.GetOrdinal("ShelveId")),
                        name: reader.GetString(reader.GetOrdinal("ShelveName")),
                        description: reader.GetString(reader.GetOrdinal("ShelveDescription")),
                        warehouse: warehouse,
                        productStocks: new List<ProductStock>(),
                        stockMovements: new List<StockMovement>()
                    );

                    result.Add(shelve);
                }
            }

            return result;
        }


    }
}
