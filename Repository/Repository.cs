using BusinessLogic.Común;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public abstract class Repository<T> where T : IEntity<object>, IAuditable
    {
        private readonly string _connectionString;
        protected readonly ILogger<T> _logger;

        protected Repository(string connectionString, ILogger<T> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected TResult ExecuteRead<TResult>(
            string baseQuery,
            Action<SqlCommand>? configureCommand,
            Func<SqlDataReader, TResult> map)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                // Agregar campos de auditoría automáticamente al SELECT
                var query = AppendAuditFields(baseQuery, AuditAction.Read);

                using var command = new SqlCommand(query, connection);
                configureCommand?.Invoke(command);

                using var reader = command.ExecuteReader();
                return map(reader);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Error de base de datos: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error inesperado.", ex);
            }
        }

        protected int ExecuteWriteWithAudit(
            string baseQuery,
            T entity,
            AuditAction action,
            Action<SqlCommand> configureCommand)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                // Construir la query con campos de auditoría correctamente insertados
                var auditQuery = AppendAuditFields(baseQuery, action);

                using var command = new SqlCommand(auditQuery, connection);

                var auditInfo = entity.AuditInfo;

                // Inyectar parámetros de auditoría según la acción
                switch (action)
                {
                    case AuditAction.Insert:
                        command.Parameters.AddWithValue("@CreatedAt", auditInfo.CreatedAt);
                        command.Parameters.AddWithValue("@CreatedBy", auditInfo.CreatedBy);
                        command.Parameters.AddWithValue("@CreatedLocation", auditInfo.CreatdedLocation);
                        break;

                    case AuditAction.Update:
                        command.Parameters.AddWithValue("@UpdatedAt", auditInfo.UpdatedAt ?? DateTime.UtcNow);
                        command.Parameters.AddWithValue("@UpdatedBy", auditInfo.UpdatedBy);
                        command.Parameters.AddWithValue("@UpdatedLocation", auditInfo.UpdatedLocation ?? "");
                        break;

                    case AuditAction.Delete:
                        command.Parameters.AddWithValue("@DeletedAt", auditInfo.DeletedAt ?? DateTime.UtcNow);
                        command.Parameters.AddWithValue("@DeletedBy", auditInfo.DeletedBy);
                        command.Parameters.AddWithValue("@DeletedLocation", auditInfo.DeletedLocation ?? "");
                        break;
                }

                // Configura parámetros adicionales de la entidad
                configureCommand(command);

                return command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Error de base de datos: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error inesperado.", ex);
            }
        }

        private string AppendAuditFields(string baseQuery, AuditAction action)
        {
            string auditFields = action switch
            {
                AuditAction.Read => "CreatedAt, CreatedBy, CreatedLocation, " +
                                    "UpdatedAt, UpdatedBy, UpdatedLocation, " +
                                    "DeletedAt, DeletedBy, DeletedLocation",
                AuditAction.Insert => "CreatedAt, CreatedBy, CreatedLocation",
                AuditAction.Update => "UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy, UpdatedLocation = @UpdatedLocation",
                AuditAction.Delete => "DeletedAt = @DeletedAt, DeletedBy = @DeletedBy, DeletedLocation = @DeletedLocation",
                _ => throw new InvalidOperationException("Acción de auditoría no soportada.")
            };

            baseQuery = baseQuery.Trim();

            if (baseQuery.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                // Insertar campos de auditoría después de SELECT
                int selectIndex = "SELECT ".Length;
                return baseQuery.Insert(selectIndex, $"{auditFields}, ");
            }

            if (baseQuery.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
            {
                // Agregar campos de auditoría en columnas y valores
                var colStart = baseQuery.IndexOf('(');
                var colEnd = baseQuery.IndexOf(')', colStart);
                var valStart = baseQuery.IndexOf("VALUES", StringComparison.OrdinalIgnoreCase);
                var valOpen = baseQuery.IndexOf('(', valStart);
                var valClose = baseQuery.IndexOf(')', valOpen);

                if (colStart == -1 || colEnd == -1 || valOpen == -1 || valClose == -1)
                    throw new InvalidOperationException("La query INSERT no tiene la sintaxis esperada.");

                var updatedColumns = baseQuery.Insert(colEnd, $", {auditFields}");
                var updatedValues = updatedColumns.Insert(valClose, $", @{auditFields.Replace(", ", ", @")}");

                return updatedValues;
            }

            if (baseQuery.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase))
            {
                var setIndex = baseQuery.IndexOf("SET", StringComparison.OrdinalIgnoreCase);
                if (setIndex == -1)
                    throw new InvalidOperationException("La query UPDATE no tiene cláusula SET.");

                var whereIndex = baseQuery.IndexOf("WHERE", StringComparison.OrdinalIgnoreCase);

                if (whereIndex != -1)
                {
                    // Insertar antes del WHERE
                    return baseQuery.Insert(whereIndex, $", {auditFields} ");
                }
                else
                {
                    // No hay WHERE, solo agregar al final del SET
                    return $"{baseQuery}, {auditFields}";
                }
            }

            throw new InvalidOperationException("Tipo de query no soportado en AppendAuditFields.");
        }
    }

    public enum AuditAction
    {
        Insert,
        Update,
        Delete,
        Read
    }
}
