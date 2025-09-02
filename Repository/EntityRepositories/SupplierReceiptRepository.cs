using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class SupplierReceiptRepository : Repository<SupplierReceipt, SupplierReceipt.UpdatableData>, ISupplierReceiptRepository
    {
        public SupplierReceiptRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        private readonly string baseQuery = @"
            SELECT
                -- Receipt
                r.Id, r.Date, r.Amount, r.Notes, r.PaymentMethod, r.SupplierId,
                -- Supplier (prefijo Supplier)
                s.Id            AS SupplierId,
                s.Name          AS SupplierName,
                s.RUT           AS SupplierRUT,
                s.RazonSocial   AS SupplierRazonSocial,
                s.Address       AS SupplierAddress,
                s.Location      AS SupplierLocation,
                s.Phone         AS SupplierPhone,
                s.ContactName   AS SupplierContactName,
                s.Email         AS SupplierEmail,
                s.Observations  AS SupplierObservations,
                -- Bank accounts (como en SupplierRepository)
                b.Id            AS BankAccountId,
                b.BankName,
                b.AccountName,
                b.AccountNumber
            FROM SupplierReceipts r
            LEFT JOIN Suppliers s            ON s.Id = r.SupplierId
            LEFT JOIN SupplierBankAccounts b ON b.SupplierId = s.Id
        ";

        #region Add
        public async Task<SupplierReceipt> AddAsync(SupplierReceipt receipt)
        {
            if (receipt.Supplier?.Id == null)
                throw new ArgumentNullException("SupplierId");

            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO SupplierReceipts ([Date], [Amount], [Notes], [PaymentMethod], [SupplierId])
                  OUTPUT INSERTED.Id
                  VALUES (@Date, @Amount, @Notes, @PaymentMethod, @SupplierId)",
                receipt,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Date", receipt.Date);
                    cmd.Parameters.AddWithValue("@Amount", receipt.Amount);
                    cmd.Parameters.AddWithValue("@Notes", (object?)receipt.Notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentMethod", receipt.PaymentMethod.ToString());
                    cmd.Parameters.AddWithValue("@SupplierId", receipt.Supplier!.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            return new SupplierReceipt(newId, receipt.Date, receipt.Amount, receipt.Notes, receipt.PaymentMethod, receipt.AuditInfo, receipt.Supplier);
        }
        #endregion

        #region Update
        public async Task<SupplierReceipt> UpdateAsync(SupplierReceipt receipt)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE SupplierReceipts
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
        public async Task<SupplierReceipt> DeleteAsync(SupplierReceipt receipt)
        {
            if (receipt == null) throw new ArgumentNullException(nameof(receipt));

            int rows = await ExecuteWriteWithAuditAsync(
                "DELETE FROM SupplierReceipts WHERE Id = @Id",
                receipt,
                AuditAction.Delete,
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", receipt.Id)
            );

            if (rows == 0) throw new InvalidOperationException($"No se pudo eliminar el recibo con Id {receipt.Id}");
            return receipt;
        }
        #endregion

        #region GetAll
        public async Task<List<SupplierReceipt>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "SupplierId", "PaymentMethod" };
            var dict = new Dictionary<int, SupplierReceipt>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int rid = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(rid, out var receipt))
                        {
                            receipt = SupplierReceiptMapper.FromReader(reader);
                            // preparar colección para acumular
                            receipt.Supplier!.BankAccounts = new List<BankAccount>();
                            dict.Add(rid, receipt);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                            dict[rid].Supplier!.BankAccounts!.Add(BankAccountMapper.FromReader(reader));
                    }

                    return dict.Values.ToList();
                },
                options: options,
                tableAlias: "r",
                allowedFilterColumns: allowedFilters
            );
        }
        #endregion

        #region GetById
        public async Task<SupplierReceipt?> GetByIdAsync(int id)
        {
            SupplierReceipt? result = null;

            await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE r.Id = @Id",
                map: reader =>
                {
                    while (reader.Read())
                    {
                        if (result == null)
                        {
                            result = SupplierReceiptMapper.FromReader(reader);
                            result.Supplier!.BankAccounts = new List<BankAccount>();
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("BankAccountId")))
                            result.Supplier!.BankAccounts!.Add(BankAccountMapper.FromReader(reader));
                    }
                    return result;
                },
                options: new QueryOptions(),
                tableAlias: "r",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );

            return result;
        }
        #endregion
    }
}
