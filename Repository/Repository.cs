using BusinessLogic.Común;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public abstract class Repository<T, TData> where T : IEntity<TData>, IAuditable
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
            string tableName,
            Func<SqlDataReader, TResult> map,
            Dictionary<string, string>? filters = null)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var query = $"SELECT * FROM {tableName} WHERE IsDeleted = 0";
                var parameters = new List<SqlParameter>();

                int page = 1;
                int pageSize = 20;
                string sortBy = "Id";
                string sortDirection = "ASC";

                if (filters != null)
                {
                    foreach (var kvp in filters)
                    {
                        var key = kvp.Key;
                        var value = kvp.Value;

                        if (string.IsNullOrWhiteSpace(value))
                            continue;

                        if (key.Equals("page", StringComparison.OrdinalIgnoreCase))
                        {
                            if (int.TryParse(value, out var p)) page = p;
                            continue;
                        }

                        if (key.Equals("pageSize", StringComparison.OrdinalIgnoreCase))
                        {
                            if (int.TryParse(value, out var ps)) pageSize = ps;
                            continue;
                        }

                        if (key.Equals("sortBy", StringComparison.OrdinalIgnoreCase))
                        {
                            sortBy = value;
                            continue;
                        }

                        if (key.Equals("sortDirection", StringComparison.OrdinalIgnoreCase))
                        {
                            if (value.Equals("DESC", StringComparison.OrdinalIgnoreCase))
                                sortDirection = "DESC";
                            continue;
                        }

                        var paramName = $"@{key}";
                        if (key.EndsWith("From", StringComparison.OrdinalIgnoreCase))
                        {
                            query += $" AND {key[..^4]} >= {paramName}";
                        }
                        else if (key.EndsWith("To", StringComparison.OrdinalIgnoreCase))
                        {
                            query += $" AND {key[..^2]} <= {paramName}";
                        }
                        else if (key.Equals("Id", StringComparison.OrdinalIgnoreCase) || key.EndsWith("Id", StringComparison.OrdinalIgnoreCase))
                        {
                            query += $" AND {key} = {paramName}";
                        }
                        else
                        {
                            query += $" AND {key} LIKE {paramName}";
                            value = $"%{value}%"; // Agregar comodines para LIKE
                        }

                        parameters.Add(new SqlParameter(paramName, value));
                    }
                }

                // Orden y paginación
                query += $" ORDER BY {sortBy} {sortDirection}";
                query += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
                parameters.Add(new SqlParameter("@PageSize", pageSize));

                using var command = new SqlCommand(query, connection);
                foreach (var param in parameters)
                    command.Parameters.Add(param);

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



        // ✅ Versión genérica para Insert con OUTPUT INSERTED.Id
        protected TResult ExecuteWriteWithAudit<TResult>(
            string baseQuery,
            T entity,
            AuditAction action,
            Action<SqlCommand> configureCommand,
            Func<SqlCommand, TResult> resultHandler)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var auditQuery = AppendAuditFields(baseQuery, action);

                using var command = new SqlCommand(auditQuery, connection);

                var auditInfo = entity.AuditInfo;

                switch (action)
                {
                    case AuditAction.Insert:
                        command.Parameters.AddWithValue("@CreatedAt", auditInfo.CreatedAt);
                        command.Parameters.AddWithValue("@CreatedBy", auditInfo.CreatedBy);
                        command.Parameters.AddWithValue("@CreatedLocation", auditInfo.CreatedLocation);
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

                configureCommand(command);

                return resultHandler(command);
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

        // ✅ Versión corta para Update y Delete (devuelve filas afectadas)
        protected int ExecuteWriteWithAudit(
            string baseQuery,
            T entity,
            AuditAction action,
            Action<SqlCommand> configureCommand)
        {
            return ExecuteWriteWithAudit(
                baseQuery,
                entity,
                action,
                configureCommand,
                cmd => cmd.ExecuteNonQuery()
            );
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
                int selectIndex = "SELECT ".Length;
                return baseQuery.Insert(selectIndex, $"{auditFields}, ");
            }

            if (baseQuery.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
            {
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
                    return baseQuery.Insert(whereIndex, $", {auditFields} ");
                }
                else
                {
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
