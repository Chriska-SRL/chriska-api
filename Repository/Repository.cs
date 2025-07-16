using BusinessLogic.Común;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Utils;

namespace Repository
{
    public abstract class Repository<T, TData> where T : IEntity<TData>, IAuditable
    {
        private readonly string _connectionString;
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
            QueryOptions options)
        {
            try
            {
                await using var connection = CreateConnection();
                await connection.OpenAsync();


                var queryBuilder = new QueryBuilder(baseQuery)
                    .AddIsDeletedFilter()
                    .ApplyFilters(options.Filters)
                    .ApplySorting(options.SortBy, options.SortDirection)
                    .ApplyPagination(options.Page, options.PageSize);

                using var command = new SqlCommand(queryBuilder.Sql, connection);
                command.Parameters.AddRange(queryBuilder.Parameters.ToArray());

                await using var reader = await command.ExecuteReaderAsync();
                return map(reader);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error de base de datos.", ex);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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

            switch (action)
            {
                case AuditAction.Insert:
                    command.Parameters.AddWithValue("@CreatedAt", audit.CreatedAt);
                    command.Parameters.AddWithValue("@CreatedBy", audit.CreatedBy);
                    command.Parameters.AddWithValue("@CreatedLocation", audit.CreatedLocation ?? string.Empty);
                    break;

                case AuditAction.Update:
                    command.Parameters.AddWithValue("@UpdatedAt", audit.UpdatedAt ?? DateTime.UtcNow);
                    command.Parameters.AddWithValue("@UpdatedBy", audit.UpdatedBy);
                    command.Parameters.AddWithValue("@UpdatedLocation", audit.UpdatedLocation ?? string.Empty);
                    break;

                case AuditAction.Delete:
                    command.Parameters.AddWithValue("@DeletedAt", audit.DeletedAt ?? DateTime.UtcNow);
                    command.Parameters.AddWithValue("@DeletedBy", audit.DeletedBy);
                    command.Parameters.AddWithValue("@DeletedLocation", audit.DeletedLocation ?? string.Empty);
                    break;
            }
        }

        private string AppendAuditFields(string baseQuery, AuditAction action)
        {
            return action switch
            {
                AuditAction.Insert => $"{baseQuery}, CreatedAt, CreatedBy, CreatedLocation VALUES (@CreatedAt, @CreatedBy, @CreatedLocation)",
                AuditAction.Update => $"{baseQuery}, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy, UpdatedLocation = @UpdatedLocation",
                AuditAction.Delete => $"{baseQuery}, DeletedAt = @DeletedAt, DeletedBy = @DeletedBy, DeletedLocation = @DeletedLocation",
                _ => baseQuery
            };
        }

        #endregion
    }

    public enum AuditAction
    {
        Insert, Update, Delete
    }
}
