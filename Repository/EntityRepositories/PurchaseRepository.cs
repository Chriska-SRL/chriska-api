using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;
using System.Data;

namespace Repository.EntityRepositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(string connectionString, ILogger<Purchase> logger) : base(connectionString, logger) { }

        public Purchase Add(Purchase purchase)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO Purchases (Date, Status, SupplierId)
                    OUTPUT INSERTED.Id
                    VALUES (@Date, @Status, @SupplierId)", connection);

                command.Parameters.AddWithValue("@Date", purchase.Date);
                command.Parameters.AddWithValue("@Status", purchase.Status);
                command.Parameters.AddWithValue("@SupplierId", purchase.Supplier.Id);

                connection.Open();
                var result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("No se pudo insertar la compra.");

                int purchaseId = (int)result;

                return new Purchase(purchaseId, purchase.Date, purchase.Status, purchase.Supplier);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al insertar compra.");
                throw new ApplicationException("Error SQL al insertar compra.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar compra.");
                throw new ApplicationException("Error inesperado al insertar compra.", ex);
            }
        }

        public Purchase? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var purchase = GetById(id);
                if (purchase == null) return null;

                using var command = new SqlCommand("DELETE FROM Purchases WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();

                return purchase;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al eliminar compra.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar compra.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public List<Purchase> GetAll()
        {
            try
            {
                var purchases = new List<Purchase>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT 
                        p.*, 
                        s.Id AS SupplierId, 
                        s.Name AS SupplierName
                    FROM Purchases p
                    JOIN Suppliers s ON p.SupplierId = s.Id", connection);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    purchases.Add(PurchaseMapper.FromReader(reader));
                }

                return purchases;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener compras.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener compras.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Purchase? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT 
                        p.*, 
                        s.Id AS SupplierId, 
                        s.Name AS SupplierName
                    FROM Purchases p
                    JOIN Suppliers s ON p.SupplierId = s.Id
                    WHERE p.Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return PurchaseMapper.FromReader(reader);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener compra.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener compra.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Purchase Update(Purchase purchase)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Purchases SET 
                        Date = @Date,
                        Status = @Status,
                        SupplierId = @SupplierId
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", purchase.Id);
                command.Parameters.AddWithValue("@Date", purchase.Date);
                command.Parameters.AddWithValue("@Status", purchase.Status);
                command.Parameters.AddWithValue("@SupplierId", purchase.Supplier.Id);

                int rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new InvalidOperationException("No se pudo actualizar la compra.");

                return purchase;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al actualizar compra.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar compra.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }
    }
}
