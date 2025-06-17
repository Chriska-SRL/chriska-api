using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;
using System;
using System.Collections.Generic;

namespace Repository.EntityRepositories
{
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(string connectionString, ILogger<Receipt> logger) : base(connectionString, logger)
        {
        }

        public Receipt Add(Receipt receipt)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    INSERT INTO Receipts (Date, Amount, Notes, ClientId, PaymentMethod)
                    OUTPUT INSERTED.Id
                    VALUES (@Date, @Amount, @Notes, @ClientId, @PaymentMethod)", connection);

                command.Parameters.AddWithValue("@Date", receipt.Date);
                command.Parameters.AddWithValue("@Amount", receipt.Amount);
                command.Parameters.AddWithValue("@Notes", receipt.Notes ?? "");
                command.Parameters.AddWithValue("@ClientId", receipt.Client.Id);
                command.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod);

                var result = command.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("No se pudo insertar el recibo.");

                receipt.Id = (int)result;
                return receipt;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al insertar recibo.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar recibo.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Receipt? Delete(int id)
        {
            try
            {
                var existing = GetById(id);
                if (existing == null) return null;

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("DELETE FROM Receipts WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();

                return existing;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al eliminar recibo.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar recibo.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public List<Receipt> GetAll()
        {
            try
            {
                var receipts = new List<Receipt>();

                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT 
                        r.Id, r.Date, r.Amount, r.PaymentMethod, r.Notes,
                        c.Id AS ClientId, c.Name AS ClientName, c.RUT AS ClientRUT, 
                        c.RazonSocial AS ClientRazonSocial, c.Address AS ClientAddress,
                        c.MapsAddress AS ClientMapsAddress, c.Schedule AS ClientSchedule,
                        c.Phone AS ClientPhone, c.ContactName AS ClientContactName,
                        c.Email AS ClientEmail, c.Observations AS ClientObservations,
                        c.Bank AS ClientBank, c.BankAccount AS ClientBankAccount,
                        c.LoanedCrates AS ClientLoanedCrates,
                        z.Id AS ZoneId, z.Name AS ZoneName, z.Description AS ZoneDescription
                    FROM Receipts r
                    JOIN Clients c ON r.ClientId = c.Id
                    JOIN Zones z ON c.ZoneId = z.Id", connection);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    receipts.Add(ReceiptMapper.FromReader(reader));
                }

                return receipts;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener recibos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener recibos.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Receipt? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    SELECT 
                        r.Id, r.Date, r.Amount, r.PaymentMethod, r.Notes,
                        c.Id AS ClientId, c.Name AS ClientName, c.RUT AS ClientRUT, 
                        c.RazonSocial AS ClientRazonSocial, c.Address AS ClientAddress,
                        c.MapsAddress AS ClientMapsAddress, c.Schedule AS ClientSchedule,
                        c.Phone AS ClientPhone, c.ContactName AS ClientContactName,
                        c.Email AS ClientEmail, c.Observations AS ClientObservations,
                        c.Bank AS ClientBank, c.BankAccount AS ClientBankAccount,
                        c.LoanedCrates AS ClientLoanedCrates,
                        z.Id AS ZoneId, z.Name AS ZoneName, z.Description AS ZoneDescription
                    FROM Receipts r
                    JOIN Clients c ON r.ClientId = c.Id
                    JOIN Zones z ON c.ZoneId = z.Id
                    WHERE r.Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) return null;

                return ReceiptMapper.FromReader(reader);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al obtener recibo por ID.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener recibo por ID.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Receipt Update(Receipt receipt)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Receipts SET
                        Date = @Date,
                        Amount = @Amount,
                        Notes = @Notes,
                        ClientId = @ClientId,
                        PaymentMethod = @PaymentMethod
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", receipt.Id);
                command.Parameters.AddWithValue("@Date", receipt.Date);
                command.Parameters.AddWithValue("@Amount", receipt.Amount);
                command.Parameters.AddWithValue("@Notes", receipt.Notes ?? "");
                command.Parameters.AddWithValue("@ClientId", receipt.Client.Id);
                command.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod);

                var rows = command.ExecuteNonQuery();
                if (rows == 0)
                    throw new InvalidOperationException("No se pudo actualizar el recibo.");

                return receipt;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error SQL al actualizar recibo.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar recibo.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }
    }
}
