using Microsoft.Data.SqlClient;

namespace Repository.Utils
{
    internal class QueryBuilder
    {
        public string Sql { get; private set; }
        public List<SqlParameter> Parameters { get; } = new();

        private HashSet<string>? _allowedFilterColumns;

        public QueryBuilder(string baseQuery)
        {
            Sql = baseQuery.Trim();
        }

        public QueryBuilder WithAllowedFilters(IEnumerable<string> columns)
        {
            _allowedFilterColumns = columns
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim().ToLowerInvariant())
                .ToHashSet();

            return this;
        }

        public QueryBuilder AddIsDeletedFilter(string? tableAlias = null)
        {
            var column = string.IsNullOrWhiteSpace(tableAlias) ? "IsDeleted" : $"{tableAlias}.IsDeleted";
            InsertWhereCondition($"{column} = 0");
            return this;
        }

        public QueryBuilder ApplyFilters(Dictionary<string, string>? filters, string? tableAlias)
        {
            if (filters == null) return this;

            foreach (var (key, value) in filters)
            {
                if (string.IsNullOrWhiteSpace(value)) continue;

                var rawKey = key.Trim();
                var baseKey =
                    rawKey.EndsWith("From", StringComparison.OrdinalIgnoreCase) ? rawKey[..^4] :
                    rawKey.EndsWith("To", StringComparison.OrdinalIgnoreCase) ? rawKey[..^2] :
                    rawKey;

                if (_allowedFilterColumns != null &&
                    !_allowedFilterColumns.Contains(baseKey.ToLowerInvariant()))
                {
                    continue; // Filtro no permitido
                }

                var paramName = $"@p{Parameters.Count}";
                var columnSql = tableAlias is null ? baseKey : $"{tableAlias}.{baseKey}";

                string condition =
                    rawKey.EndsWith("From", StringComparison.OrdinalIgnoreCase) ? $"{columnSql} >= {paramName}" :
                    rawKey.EndsWith("To", StringComparison.OrdinalIgnoreCase) ? $"{columnSql} <= {paramName}" :
                    $"{columnSql} LIKE {paramName}";

                InsertWhereCondition(condition);

                var paramValue =
                    rawKey.EndsWith("From", StringComparison.OrdinalIgnoreCase) ||
                    rawKey.EndsWith("To", StringComparison.OrdinalIgnoreCase)
                        ? value
                        : $"%{value}%";

                Parameters.Add(new SqlParameter(paramName, paramValue));
            }

            return this;
        }


        public QueryBuilder ApplySorting(string? sortBy, string? sortDirection)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // validación opcional
                if (_allowedFilterColumns != null &&
                    !_allowedFilterColumns.Contains(sortBy.Trim().ToLowerInvariant()))
                    return this;

                var safeSortDir = sortDirection?.ToUpperInvariant() == "DESC" ? "DESC" : "ASC";
                Sql += $" ORDER BY {sortBy} {safeSortDir}";
            }

            return this;
        }

        public QueryBuilder ApplyPagination(int page, int pageSize)
        {
            if (!Sql.Contains("ORDER BY", StringComparison.OrdinalIgnoreCase))
            {
                // Evita OFFSET sin ORDER BY
                Sql += " ORDER BY (SELECT NULL)";
            }

            Sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            Parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
            Parameters.Add(new SqlParameter("@PageSize", pageSize));

            return this;
        }

        public QueryBuilder AddAuditColumns(string? tableAlias)
        {
            if (string.IsNullOrWhiteSpace(tableAlias)) return this;

            var auditCols = new[]
            {
                "CreatedAt", "CreatedBy", "CreatedLocation",
                "UpdatedAt", "UpdatedBy", "UpdatedLocation",
                "DeletedAt", "DeletedBy", "DeletedLocation"
            };

            var fromIndex = Sql.IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
            if (fromIndex == -1) return this;

            var selectClause = Sql.Substring(0, fromIndex).Trim();
            var restOfQuery = Sql.Substring(fromIndex);

            if (!selectClause.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                return this;

            if (selectClause.Contains("(", StringComparison.OrdinalIgnoreCase) &&
                !selectClause.Contains(","))
                return this;

            foreach (var col in auditCols)
            {
                var fullCol = $"{tableAlias}.{col}";
                if (!selectClause.Contains(fullCol, StringComparison.OrdinalIgnoreCase))
                    selectClause += $", {fullCol}";
            }

            Sql = $"{selectClause} {restOfQuery}";
            return this;
        }

        #region Helpers

        private void InsertWhereCondition(string condition)
        {
            var groupByIndex = IndexOfClause("GROUP BY");
            var orderByIndex = IndexOfClause("ORDER BY");
            var insertPos = groupByIndex >= 0 ? groupByIndex :
                            orderByIndex >= 0 ? orderByIndex : Sql.Length;

            if (Sql.Contains("WHERE", StringComparison.OrdinalIgnoreCase))
            {
                Sql = Sql.Insert(insertPos, $" AND {condition} ");
            }
            else
            {
                Sql = Sql.Insert(insertPos, $" WHERE {condition} ");
            }
        }

        private int IndexOfClause(string clause)
        {
            return Sql.IndexOf(clause, StringComparison.OrdinalIgnoreCase);
        }

        private bool ContainsClause(string clause)
        {
            return Sql.IndexOf(clause, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        #endregion
    }
}
