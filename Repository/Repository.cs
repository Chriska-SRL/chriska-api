using BusinessLogic.Común;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Utils;

namespace Repository
{
    public abstract class Repository<T, TData> where T : IEntity<TData>, IAuditable
    {
        public readonly string _connectionString;
        private readonly AuditLogger _auditLogger;

        protected Repository(string connectionString, AuditLogger auditLogger)
        {
            _connectionString = connectionString;
            _auditLogger = auditLogger;
        }

        protected SqlConnection CreateConnection()
            => new SqlConnection(_connectionString);

        #region Read

        protected async Task<TResult> ExecuteReadAsync<TResult>(
    string baseQuery,
    Func<SqlDataReader, TResult> map,
    QueryOptions options,
    string? tableAlias = null,
    Action<SqlCommand>? configureCommand = null,
    IEnumerable<string>? allowedFilterColumns = null)
        {
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();

                var queryBuilder = new QueryBuilder(baseQuery)
                    .AddAuditColumns(tableAlias)
                    .AddIsDeletedFilter(tableAlias)
                    .WithAllowedFilters(allowedFilterColumns ?? Array.Empty<string>())
                    .ApplyFilters(options.Filters)
                    .ApplySorting(options.SortBy, options.SortDirection)
                    .ApplyPagination(options.Page, options.PageSize);

                using var command = new SqlCommand(queryBuilder.Sql, connection);
                command.Parameters.AddRange(queryBuilder.Parameters.ToArray());

                configureCommand?.Invoke(command);

                await using var reader = await command.ExecuteReaderAsync();

                Console.WriteLine(command.CommandText);
                foreach (SqlParameter p in command.Parameters)
                    Console.WriteLine($"{p.ParameterName} = {p.Value}");

                return map(reader);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error de base de datos.", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Write

        protected async Task<TResult> ExecuteWriteWithAuditAsync<TResult>(
            string baseQuery,
            T entity,
            AuditAction action,
            Action<SqlCommand> configureCommand,
            Func<SqlCommand, Task<TResult>> resultHandler)
        {
            TResult result;
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();

                var finalQuery = AppendAuditFields(baseQuery, action);

                using var command = new SqlCommand(finalQuery, connection);

                AddAuditParameters(command, entity, action);
                configureCommand(command);

                result = await resultHandler(command);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error de base de datos.", ex);
            }
            catch (Exception)
            {
                throw;
            }

            await _auditLogger.LogAsync(
                entityName: typeof(T).Name,
                entityId: entity.Id,
                action: action.ToString(),
                auditInfo: entity.AuditInfo
            );

            return result;
        }

        protected Task<int> ExecuteWriteWithAuditAsync(
            string baseQuery,
            T entity,
            AuditAction action,
            Action<SqlCommand> configureCommand)
        {
            return ExecuteWriteWithAuditAsync(
                baseQuery, entity, action, configureCommand,
                async cmd => await cmd.ExecuteNonQueryAsync());
        }

        #endregion

        #region Helpers

        private void AddAuditParameters(SqlCommand command, T entity, AuditAction action)
        {
            var audit = entity.AuditInfo;

            object? AsDbValue(object? value) => value ?? DBNull.Value;

            switch (action)
            {
                case AuditAction.Insert:
                    command.Parameters.AddWithValue("@CreatedAt", AsDbValue(audit.CreatedAt));
                    command.Parameters.AddWithValue("@CreatedBy", AsDbValue(audit.CreatedBy));
                    command.Parameters.AddWithValue("@CreatedLocation", AsDbValue(audit.CreatedLocation?.ToString()));
                    break;

                case AuditAction.Update:
                    command.Parameters.AddWithValue("@UpdatedAt", AsDbValue(audit.UpdatedAt ?? DateTime.UtcNow));
                    command.Parameters.AddWithValue("@UpdatedBy", AsDbValue(audit.UpdatedBy));
                    command.Parameters.AddWithValue("@UpdatedLocation", AsDbValue(audit.UpdatedLocation?.ToString()));
                    break;

                case AuditAction.Delete:
                    command.Parameters.AddWithValue("@DeletedAt", AsDbValue(audit.DeletedAt ?? DateTime.UtcNow));
                    command.Parameters.AddWithValue("@DeletedBy", AsDbValue(audit.DeletedBy));
                    command.Parameters.AddWithValue("@DeletedLocation", AsDbValue(audit.DeletedLocation?.ToString()));
                    break;
            }
        }


        private string AppendAuditFields(string baseQuery, AuditAction action)
        {
            switch (action)
            {
                case AuditAction.Insert:
                    return AppendInsertAuditFields(baseQuery);

                case AuditAction.Update:
                    // Busca el índice del SET para insertar campos de auditoría justo después
                    var setIndex = baseQuery.IndexOf("SET", StringComparison.OrdinalIgnoreCase);
                    if (setIndex == -1)
                        throw new InvalidOperationException("No se encontró la cláusula SET en la consulta UPDATE.");

                    var afterSet = baseQuery.Substring(setIndex + 3).Trim();
                    var beforeSet = baseQuery.Substring(0, setIndex + 3);

                    var auditSet = " UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy, UpdatedLocation = @UpdatedLocation";
                    if (!string.IsNullOrWhiteSpace(afterSet))
                        auditSet += ", " + afterSet;

                    return $"{beforeSet} {auditSet}";

                case AuditAction.Delete:
                    // Similar al Update pero para campos de borrado
                    setIndex = baseQuery.IndexOf("SET", StringComparison.OrdinalIgnoreCase);
                    if (setIndex == -1)
                        throw new InvalidOperationException("No se encontró la cláusula SET en la consulta DELETE.");

                    afterSet = baseQuery.Substring(setIndex + 3).Trim();
                    beforeSet = baseQuery.Substring(0, setIndex + 3);

                    var deleteAuditSet = " DeletedAt = @DeletedAt, DeletedBy = @DeletedBy, DeletedLocation = @DeletedLocation";
                    if (!string.IsNullOrWhiteSpace(afterSet))
                        deleteAuditSet += ", " + afterSet;

                    return $"{beforeSet} {deleteAuditSet}";

                default:
                    return baseQuery;
            }
        }

        private string AppendInsertAuditFields(string baseQuery)
        {
            // Espera: "INSERT INTO tabla (Col1, Col2) OUTPUT INSERTED.Id VALUES (@Col1, @Col2)"
            int parOpenCols = baseQuery.IndexOf('(');
            int parCloseCols = baseQuery.IndexOf(')', parOpenCols);
            int valuesIndex = baseQuery.IndexOf("VALUES", StringComparison.OrdinalIgnoreCase);
            int parOpenVals = baseQuery.IndexOf('(', valuesIndex);
            int parCloseVals = baseQuery.IndexOf(')', parOpenVals);

            if (parOpenCols == -1 || parCloseCols == -1 || parOpenVals == -1 || parCloseVals == -1)
                throw new InvalidOperationException("No se pudo analizar la consulta INSERT para agregar campos de auditoría.");

            string beforeCols = baseQuery.Substring(0, parCloseCols);
            string afterCols = baseQuery.Substring(parCloseCols, parOpenVals - parCloseCols);
            string beforeVals = baseQuery.Substring(parOpenVals, parCloseVals - parOpenVals);
            string afterVals = baseQuery.Substring(parCloseVals);

            string auditCols = ", CreatedAt, CreatedBy, CreatedLocation";
            string auditVals = ", @CreatedAt, @CreatedBy, @CreatedLocation";

            return beforeCols + auditCols + afterCols + beforeVals + auditVals + afterVals;
        }

        #endregion
    }

    public enum AuditAction
    {
        Insert, Update, Delete
    }
}
