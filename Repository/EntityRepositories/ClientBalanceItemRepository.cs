using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using System.Data;

namespace Repository.EntityRepositories
{
    public class ClientBalanceItemRepository : IRepository<BalanceItem>, IClientBalanceItemRepository
    {
        private readonly string _connectionString;
        private readonly AuditLogger _auditLogger;

        public ClientBalanceItemRepository(string connectionString, AuditLogger auditLogger)
        {
            _connectionString = connectionString;
            _auditLogger = auditLogger;
        }

        public async Task<BalanceItem> AddAsync(BalanceItem item)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand(
                @"INSERT INTO ClientBalanceItems (ClientId, Date, Description, Amount, Balance, DocumentType)
                  OUTPUT INSERTED.Id
                  VALUES (@ClientId, @Date, @Description, @Amount, @Balance, @DocumentType)", connection);

            cmd.Parameters.AddWithValue("@ClientId", item.EntityId);
            cmd.Parameters.AddWithValue("@Date", item.Date);
            cmd.Parameters.AddWithValue("@Description", (object?)item.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Amount", item.Amount);
            cmd.Parameters.AddWithValue("@Balance", item.Balance);
            cmd.Parameters.AddWithValue("@DocumentType", item.DocumentType.ToString());

            item.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return item;
        }

        public async Task<BalanceItem?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand("SELECT * FROM ClientBalanceItems WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return Map(reader);

            return null;
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

            var cmd = new SqlCommand("DELETE FROM ClientBalanceItems WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", item.Id);

            await cmd.ExecuteNonQueryAsync();
            return item;
        }

        public async Task<List<BalanceItem>> GetByClientIdAsync(int clientId, DateTime? from = null, DateTime? to = null)
        {
            var result = new List<BalanceItem>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"SELECT * FROM ClientBalanceItems WHERE ClientId = @ClientId";
            if (from.HasValue)
                query += " AND Date >= @From";
            if (to.HasValue)
                query += " AND Date <= @To";
            query += " ORDER BY Date ASC, Id ASC";

            var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@ClientId", clientId);
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
            var documentType = Enum.Parse<DocumentType>(reader["DocumentType"].ToString() ?? "ClientPayment");
            int relatedId = reader.GetInt32(reader.GetOrdinal("EntityId"));

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