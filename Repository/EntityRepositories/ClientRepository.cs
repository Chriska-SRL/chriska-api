using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Client> logger) : base(connectionString, logger)
        {
        }

        public Client Add(Client client)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
            INSERT INTO Clients 
            (Name, RazonSocial, RUT, Address, MapsAddress, Phone, Email, ContactName, Schedule, Bank, BankAccount, LoanedCrates, Observations, ZoneId)
            OUTPUT INSERTED.Id 
            VALUES 
            (@Name, @RazonSocial, @RUT, @Address, @MapsAddress, @Phone, @Email, @ContactName, @Schedule, @Bank, @BankAccount, @LoanedCrates, @Observations, @ZoneId)", connection);

                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@RazonSocial", client.RazonSocial);
                command.Parameters.AddWithValue("@RUT", client.RUT);
                command.Parameters.AddWithValue("@Address", client.Address);
                command.Parameters.AddWithValue("@MapsAddress", client.MapsAddress);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@ContactName", client.ContactName);
                command.Parameters.AddWithValue("@Schedule", client.Schedule);
                command.Parameters.AddWithValue("@Bank", client.Bank.ToString()); 
                command.Parameters.AddWithValue("@BankAccount", client.BankAccount);
                command.Parameters.AddWithValue("@LoanedCrates", client.LoanedCrates);
                command.Parameters.AddWithValue("@Observations", client.Observations);
                command.Parameters.AddWithValue("@ZoneId", client.Zone.Id);

                connection.Open();
                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("No se pudo insertar el cliente.");

                int clientId = (int)result;

                return new Client(clientId, client.Name, client.RUT, client.RazonSocial, client.Address, client.MapsAddress,
                    client.Schedule, client.Phone, client.ContactName, client.Email, client.Observations, client.Bank, client.BankAccount, client.LoanedCrates, client.Zone);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"SQL Error al insertar cliente: {ex.Message} \nStackTrace: {ex.StackTrace}");
                throw new ApplicationException($"Error SQL al insertar cliente: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inesperado al insertar cliente: {ex.Message} \nStackTrace: {ex.StackTrace}");
                throw new ApplicationException($"Error inesperado al insertar cliente: {ex.Message}", ex);
            }
        }




        public Client? Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Intentando eliminar el cliente con ID {id}.");
                using var connection = CreateConnection();
                connection.Open();

                var client = GetById(id);
                if (client == null)
                {
                    _logger.LogWarning($"No se encontró el cliente con ID {id} para eliminar.");
                    return null;
                }

                using (var command = new SqlCommand("DELETE FROM Clients WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                _logger.LogInformation($"Cliente con ID {id} eliminado correctamente.");
                return client;
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

        public List<Client> GetAll()
        {
            try
            {
                var clients = new List<Client>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
            SELECT 
                c.*, 
                z.Id AS ZoneId, 
                z.Name AS ZoneName, 
                z.Description AS ZoneDescription
            FROM Clients c
            JOIN Zones z ON c.ZoneId = z.Id
            ", connection);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(ClientMapper.FromReader(reader));
                }

                return clients;
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


        public Client? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
            SELECT 
                c.*, 
                z.Id AS ZoneId,
                z.Name AS ZoneName,
                z.Description AS ZoneDescription
            FROM Clients c
            JOIN Zones z ON c.ZoneId = z.Id
            WHERE c.Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return ClientMapper.FromReader(reader);
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



        public Client GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
            SELECT 
                c.*
            FROM Clients c
            WHERE c.Name = @Name", connection);

                command.Parameters.AddWithValue("@Name", name);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return ClientMapper.FromReader(reader);
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


        public Client GetByRUT(string rut)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
        SELECT 
            c.*, 
            z.Id AS ZoneId, 
            z.Name AS ZoneName, 
            z.Description AS ZoneDescription
        FROM Clients c
        JOIN Zones z ON c.ZoneId = z.Id
        WHERE c.RUT = @RUT", connection);

                command.Parameters.AddWithValue("@RUT", rut);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return ClientMapper.FromReader(reader);
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


        public Client Update(Client client)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
            UPDATE Clients SET
                Name = @Name,
                RUT = @RUT,
                RazonSocial = @RazonSocial,
                Address = @Address,
                MapsAddress = @MapsAddress,
                Schedule = @Schedule,
                Phone = @Phone,
                ContactName = @ContactName,
                Email = @Email,
                Observations = @Observations,
                Bank=@Bank,
                BankAccount = @BankAccount,
                LoanedCrates = @LoanedCrates,
                ZoneId = @ZoneId
            WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", client.Id);
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@RUT", client.RUT);
                command.Parameters.AddWithValue("@RazonSocial", client.RazonSocial);
                command.Parameters.AddWithValue("@Address", client.Address);
                command.Parameters.AddWithValue("@MapsAddress", client.MapsAddress);
                command.Parameters.AddWithValue("@Schedule", client.Schedule);
                command.Parameters.AddWithValue("@Phone", client.Phone);
                command.Parameters.AddWithValue("@ContactName", client.ContactName);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Observations", client.Observations);
                command.Parameters.AddWithValue("@Bank", client.Bank);
                command.Parameters.AddWithValue("@BankAccount", client.BankAccount);
                command.Parameters.AddWithValue("@LoanedCrates", client.LoanedCrates);
                command.Parameters.AddWithValue("@ZoneId", client.Zone.Id);

                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new InvalidOperationException("No se pudo actualizar el cliente.");

                return client;
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
