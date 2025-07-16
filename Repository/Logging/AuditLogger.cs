using BusinessLogic.Común;
using Microsoft.Data.SqlClient;

namespace Repository.Logging
{
    public class AuditLogger
    {
        private readonly string _connectionString;

        public AuditLogger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task LogAsync(string entityName, int entityId, string action, AuditInfo auditInfo)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand(@"
        INSERT INTO AuditLog (EntityName, EntityId, Action, ChangedAt, ChangedBy, ChangedLocation)
        VALUES (@EntityName, @EntityId, @Action, @ChangedAt, @ChangedBy, @ChangedLocation)", connection);

            cmd.Parameters.AddWithValue("@EntityName", entityName);
            cmd.Parameters.AddWithValue("@EntityId", entityId);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@ChangedAt", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@ChangedBy", auditInfo.UpdatedBy);
            cmd.Parameters.AddWithValue("@ChangedLocation", auditInfo.UpdatedLocation ?? "");

            await cmd.ExecuteNonQueryAsync();
        }

    }
}
