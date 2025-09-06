using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using System.Data;

namespace Repository.EntityRepositories
{
    public class SupplierBalanceItemRepository : IRepository<BalanceItem>, ISupplierBalanceItemRepository
    {
        private readonly string _connectionString;
        private readonly AuditLogger _auditLogger;

        public SupplierBalanceItemRepository(string connectionString, AuditLogger auditLogger)
        {
            _connectionString = connectionString;
            _auditLogger = auditLogger;
        }

        public async Task<BalanceItem> AddAsync(BalanceItem item)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand(
                @"INSERT INTO SupplierBalanceItems (SupplierId, Date, Description, Amount, Balance, DocumentType, DocumentId)
                  OUTPUT INSERTED.Id
                  VALUES (@SupplierId, @Date, @Description, @Amount, @Balance, @DocumentType, @DocumentId)", connection);

            cmd.Parameters.AddWithValue("@SupplierId", item.EntityId);
            cmd.Parameters.AddWithValue("@Date", item.Date);
            cmd.Parameters.AddWithValue("@Description", (object?)item.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Amount", item.Amount);
            cmd.Parameters.AddWithValue("@Balance", item.Balance);
            cmd.Parameters.AddWithValue("@DocumentType", item.DocumentType.ToString());
            cmd.Parameters.AddWithValue("@DocumentId", item.DocumentId);

            item.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return item;
        }

        public async Task<BalanceItem?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BalanceItem>> GetAllAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<BalanceItem> UpdateAsync(BalanceItem item)
        {
           throw new NotImplementedException();
        }

        public async Task<BalanceItem> DeleteAsync(BalanceItem item)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand("DELETE FROM SupplierBalanceItems WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", item.Id);

            await cmd.ExecuteNonQueryAsync();
            return item;
        }

        public async Task<List<BalanceItem>> GetBySupplierIdAsync(int supplierId, DateTime? from = null, DateTime? to = null)
        {
            var result = new List<BalanceItem>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"SELECT * FROM SupplierBalanceItems WHERE SupplierId = @SupplierId";
            if (from.HasValue)
                query += " AND Date >= @From";
            if (to.HasValue)
                query += " AND Date <= @To";
            query += " ORDER BY Date ASC, Id ASC";

            var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@SupplierId", supplierId);
            if (from.HasValue)
                cmd.Parameters.AddWithValue("@From", from.Value);
            if (to.HasValue)
                cmd.Parameters.AddWithValue("@To", to.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                result.Add(Map(reader));
            return result;
        }

        private BalanceItem Map(SqlDataReader reader)
        {
            var documentType = Enum.Parse<DocumentType>(reader["DocumentType"].ToString() ?? "SupplierPayment");
            int relatedId = reader.GetInt32(reader.GetOrdinal("SupplierId"));

            switch (documentType)
            {
                case DocumentType.SupplierPayment:
                case DocumentType.ClientPayment:
                    return new ReceiptBalanceItem(
                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        entityId: relatedId,
                        date: reader.GetDateTime(reader.GetOrdinal("Date")),
                        description: reader["Description"] as string,
                        amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                        balance: reader.GetDecimal(reader.GetOrdinal("Balance")),
                        documentType: documentType
                    );
                default:
                    return new DocumentBalanceItem(
                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        entityId: relatedId,
                        date: reader.GetDateTime(reader.GetOrdinal("Date")),
                        description: reader["Description"] as string,
                        amount: reader.GetDecimal(reader.GetOrdinal("Amount")),
                        balance: reader.GetDecimal(reader.GetOrdinal("Balance")),
                        documentType: documentType
                    );
            }
        }
    }
}