using BusinessLogic.Common;
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
            if (auditInfo == null)
                throw new ArgumentNullException(nameof(auditInfo), "AuditInfo no puede ser null.");

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand(@"
                INSERT INTO AuditLog (EntityName, EntityId, Action, ChangedAt, ChangedBy, ChangedLocation)
                VALUES (@EntityName, @EntityId, @Action, @ChangedAt, @ChangedBy, @ChangedLocation)", connection);

            cmd.Parameters.AddWithValue("@EntityName", entityName);
            cmd.Parameters.AddWithValue("@EntityId", entityId);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@ChangedAt", DateTime.UtcNow);

            // Si es una actualización, usá UpdatedBy, sino CreatedBy. Por defecto 0 si no hay valor.
            cmd.Parameters.AddWithValue("@ChangedBy", auditInfo.UpdatedBy ?? auditInfo.CreatedBy ?? 0);

            // Location: Updated > Created > "unknown"
            var location = auditInfo.UpdatedLocation?.ToString()
                        ?? auditInfo.CreatedLocation?.ToString()
                        ?? "unknown";
            cmd.Parameters.AddWithValue("@ChangedLocation", location);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
