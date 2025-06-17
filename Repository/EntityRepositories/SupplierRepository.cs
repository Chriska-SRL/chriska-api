using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class SupplierRepository : Repository<SupplierRepository>, ISupplierRepository
    {
        public SupplierRepository(string connectionString, ILogger<SupplierRepository> logger) : base(connectionString, logger) { }

        public Supplier Add(Supplier supplier)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO Suppliers 
                    (Name, RazonSocial, RUT, ContactName, Phone, Email, Address, mapsAddress, Bank, BankAccount, Observations) 
                    OUTPUT INSERTED.Id 
                    VALUES 
                    (@Name, @RazonSocial, @RUT, @ContactName, @Phone, @Email, @Address, @MapsAddress, @Bank, @BankAccount, @Observations)",
                    connection);

                command.Parameters.AddWithValue("@Name", supplier.Name);
                command.Parameters.AddWithValue("@RazonSocial", supplier.RazonSocial);
                command.Parameters.AddWithValue("@RUT", supplier.RUT);
                command.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                command.Parameters.AddWithValue("@Phone", supplier.Phone);
                command.Parameters.AddWithValue("@Email", supplier.Email);
                command.Parameters.AddWithValue("@Address", supplier.Address);
                command.Parameters.AddWithValue("@MapsAddress", supplier.MapsAddress);
                command.Parameters.AddWithValue("@Bank", supplier.Bank);
                command.Parameters.AddWithValue("@BankAccount", supplier.BankAccount);
                command.Parameters.AddWithValue("@Observations", supplier.Observations);

                connection.Open();
                int id = (int)command.ExecuteScalar();
                supplier.Id = id;
                return supplier;
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

        public Supplier? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var supplier = GetById(id);
                if (supplier == null) return null;

                using var command = new SqlCommand("DELETE FROM Suppliers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();

                return supplier;
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

        public List<Supplier> GetAll()
        {
            try
            {
                var suppliers = new List<Supplier>();
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT * FROM Suppliers", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    suppliers.Add(SupplierMapper.FromReader(reader));
                }

                return suppliers;
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

        public Supplier? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT * FROM Suppliers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return SupplierMapper.FromReader(reader);
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

        public Supplier? GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand(@"
                    SELECT 
                        s.*
                    FROM Suppliers s
                    WHERE s.Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    return SupplierMapper.FromReader(reader);
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

        public Supplier? GetByRUT(string rut)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand(@"
            SELECT 
                s.*
            FROM Suppliers s
            WHERE s.RUT = @RUT", connection))
                {
                    command.Parameters.AddWithValue("@RUT", rut);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    return SupplierMapper.FromReader(reader);
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

        public Supplier Update(Supplier supplier)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Suppliers SET 
                        Name = @Name, RazonSocial = @RazonSocial, RUT = @RUT, 
                        ContactName = @ContactName, Phone = @Phone, Email = @Email, 
                        Address = @Address, mapsAddress = @MapsAddress, Bank = @Bank, 
                        BankAccount = @BankAccount, Observations = @Observations
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", supplier.Id);
                command.Parameters.AddWithValue("@Name", supplier.Name);
                command.Parameters.AddWithValue("@RazonSocial", supplier.RazonSocial);
                command.Parameters.AddWithValue("@RUT", supplier.RUT);
                command.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                command.Parameters.AddWithValue("@Phone", supplier.Phone);
                command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);
                command.Parameters.AddWithValue("@MapsAddress", (object?)supplier.MapsAddress ?? DBNull.Value);
                command.Parameters.AddWithValue("@Bank", supplier.Bank);
                command.Parameters.AddWithValue("@BankAccount", supplier.BankAccount);
                command.Parameters.AddWithValue("@Observations", (object?)supplier.Observations ?? DBNull.Value);

                command.ExecuteNonQuery();

                return supplier;
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
