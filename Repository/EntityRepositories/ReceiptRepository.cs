using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ReceiptRepository : Repository<Receipt, Receipt.UpdatableData>, IReceiptRepository
    {
        public ReceiptRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Receipt> AddAsync(Receipt receipt)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO Receipts (Date, Amount, Notes, PaymentMethod, ClientId)
                  OUTPUT INSERTED.Id VALUES (@Date, @Amount, @Notes, @PaymentMethod, @ClientId)",
                receipt,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Date", receipt.Date);
                    cmd.Parameters.AddWithValue("@Amount", receipt.Amount);
                    cmd.Parameters.AddWithValue("@Notes", (object?)receipt.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod.ToString());
                    cmd.Parameters.AddWithValue("@ClientId", receipt.Client?.Id ?? throw new ArgumentNullException("ClientId"));
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new Receipt(newId, receipt.Date, receipt.Amount, receipt.Notes, receipt.PaymentMethod, receipt.Client, receipt.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Receipt> UpdateAsync(Receipt receipt)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Receipts SET Notes = @Notes, PaymentMethod = @PaymentMethod WHERE Id = @Id",
                receipt,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", receipt.Id);
                    cmd.Parameters.AddWithValue("@Notes", (object?)receipt.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod.ToString());
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el recibo con Id {receipt.Id}");

            return receipt;
        }

        #endregion

        #region Delete

        public async Task<Receipt> DeleteAsync(Receipt receipt)
        {
            if (receipt == null)
                throw new ArgumentNullException("El recibo no puede ser nulo.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Receipts SET IsDeleted = 1 WHERE Id = @Id",
                receipt,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", receipt.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el recibo con Id {receipt.Id}");

            return receipt;
        }

        #endregion

        #region Query (base)

        private readonly string baseQuery = @"
     SELECT
         -- Receipt
         r.Id,
         r.Date,
         r.Amount,
         r.Notes,
         r.PaymentMethod,
         r.ClientId,

         -- Client (prefijo Client)
         c.Id           AS ClientId,
         c.Name         AS ClientName,
         c.RUT          AS ClientRUT,
         c.RazonSocial  AS ClientRazonSocial,
         c.Address      AS ClientAddress,
         c.Location     AS ClientLocation,
         c.Schedule     AS ClientSchedule,
         c.Phone        AS ClientPhone,
         c.ContactName  AS ClientContactName,
         c.Email        AS ClientEmail,
         c.Observations AS ClientObservations,
         c.LoanedCrates AS ClientLoanedCrates,
         c.Qualification AS ClientQualification,
         c.ZoneId       AS ZoneId,

         -- Zone (prefijo Zone)
         z.Id           AS ZoneId,
         z.Name         AS ZoneName,
         z.Description  AS ZoneDescription,
         z.DeliveryDays AS ZoneDeliveryDays,
         z.RequestDays  AS ZoneRequestDays,
         z.ImageUrl     AS ZoneImageUrl

     FROM Receipts r
     LEFT JOIN Clients c ON c.Id = r.ClientId
     LEFT JOIN Zones z   ON c.ZoneId = z.Id
";


        #endregion

        #region GetAll

        public async Task<List<Receipt>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "ClientId", "PaymentMethod" };

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    var receipts = new List<Receipt>();
                    while (reader.Read())
                    {
                        receipts.Add(ReceiptMapper.FromReader(reader));
                    }
                    return receipts;
                },
                options: options,
                tableAlias: "r",
                allowedFilterColumns: allowedFilters
            );
        }

        #endregion

        #region GetById

        public async Task<Receipt?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE r.Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return ReceiptMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                tableAlias: "r",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        #endregion
    }
}
