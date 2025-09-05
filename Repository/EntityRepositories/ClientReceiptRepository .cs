using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ClientReceiptRepository : Repository<Receipt, Receipt.UpdatableData>, IClientReceiptRepository
    {
        public ClientReceiptRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        private readonly string baseQuery = @"
            SELECT
                -- Receipt
                r.Id, r.Date, r.Amount, r.Notes, r.PaymentMethod, r.ClientId,
                -- Client (prefijo Client)
                c.Id AS ClientId, c.Name AS ClientName, c.RUT AS ClientRUT, c.RazonSocial AS ClientRazonSocial,
                c.Address AS ClientAddress, c.Location AS ClientLocation, c.Schedule AS ClientSchedule,
                c.Phone AS ClientPhone, c.ContactName AS ClientContactName, c.Email AS ClientEmail,
                c.Observations AS ClientObservations, c.LoanedCrates AS ClientLoanedCrates,
                c.Qualification AS ClientQualification, c.ZoneId AS ZoneId,
                -- Zone (prefijo Zone)
                z.Id AS ZoneId, z.Name AS ZoneName, z.Description AS ZoneDescription,
                z.DeliveryDays AS ZoneDeliveryDays, z.RequestDays AS ZoneRequestDays, z.ImageUrl AS ZoneImageUrl
            FROM ClientReceipts r
            LEFT JOIN Clients c ON c.Id = r.ClientId
            LEFT JOIN Zones   z ON z.Id = c.ZoneId
            ";

        #region Add
        public async Task<ClientReceipt> AddAsync(ClientReceipt receipt)
        {
            if (receipt.Client?.Id == null)
                throw new ArgumentException("ClientId");

            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO ClientReceipts ([Date], [Amount], [Notes], [PaymentMethod], [ClientId])
                  OUTPUT INSERTED.Id
                  VALUES (@Date, @Amount, @Notes, @PaymentMethod, @ClientId)",
                receipt,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Date", receipt.Date);
                    cmd.Parameters.AddWithValue("@Amount", receipt.Amount);
                    cmd.Parameters.AddWithValue("@Notes", (object?)receipt.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod.ToString());
                    cmd.Parameters.AddWithValue("@ClientId", receipt.Client!.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0) throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new ClientReceipt(newId, receipt.Date, receipt.Amount, receipt.Notes, receipt.PaymentMethod, receipt.AuditInfo, receipt.Client);
        }
        #endregion

        #region Update
        public async Task<ClientReceipt> UpdateAsync(ClientReceipt receipt)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE ClientReceipts
                  SET Notes = @Notes, PaymentMethod = @PaymentMethod
                  WHERE Id = @Id",
                receipt,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", receipt.Id);
                    cmd.Parameters.AddWithValue("@Notes", (object?)receipt.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod.ToString());
                }
            );

            if (rows == 0) throw new InvalidOperationException($"No se pudo actualizar el recibo con Id {receipt.Id}");
            return receipt;
        }
        #endregion

        #region Delete
        public async Task<ClientReceipt> DeleteAsync(ClientReceipt receipt)
        {
            if (receipt == null) throw new ArgumentException(nameof(receipt));

            int rows = await ExecuteWriteWithAuditAsync(
                "DELETE FROM ClientReceipts WHERE Id = @Id",
                receipt,
                AuditAction.Delete,
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", receipt.Id)
            );

            if (rows == 0) throw new InvalidOperationException($"No se pudo eliminar el recibo con Id {receipt.Id}");
            return receipt;
        }
        #endregion

        #region GetAll
        public async Task<List<ClientReceipt>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "ClientId", "PaymentMethod" };

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    var list = new List<ClientReceipt>();
                    while (reader.Read()) list.Add(ClientReceiptMapper.FromReader(reader));
                    return list;
                },
                options: options,
                tableAlias: "r",
                allowedFilterColumns: allowedFilters
            );
        }
        #endregion

        #region GetById
        public async Task<ClientReceipt?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE r.Id = @Id",
                map: reader => reader.Read() ? ClientReceiptMapper.FromReader(reader) : null,
                options: new QueryOptions(),
                tableAlias: "r",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }
        #endregion
    }
}
