using Microsoft.Data.SqlClient;

namespace Repository.Utils
{
    internal class QueryBuilder
    {
        public string Sql { get; private set; }
        public List<SqlParameter> Parameters { get; } = new();

        public QueryBuilder(string baseQuery)
        {
            Sql = baseQuery.Trim();
        }

        public QueryBuilder AddIsDeletedFilter()
        {
            InsertWhereCondition("IsDeleted = 0");
            return this;
        }

        public QueryBuilder ApplyFilters(Dictionary<string, string>? filters)
        {
            if (filters == null) return this;

            foreach (var (key, value) in filters)
            {
                if (string.IsNullOrWhiteSpace(value)) continue;

                var paramName = $"@{key}";
                string condition = key.EndsWith("From", StringComparison.OrdinalIgnoreCase)
                    ? $"{key[..^4]} >= {paramName}"
                    : key.EndsWith("To", StringComparison.OrdinalIgnoreCase)
                    ? $"{key[..^2]} <= {paramName}"
                    : $"{key} LIKE {paramName}";

                InsertWhereCondition(condition);
                Parameters.Add(new SqlParameter(paramName, $"%{value}%"));
            }

            return this;
        }

        public QueryBuilder ApplySorting(string? sortBy, string? sortDirection)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var safeSortDir = sortDirection?.ToUpperInvariant() == "DESC" ? "DESC" : "ASC";

                // Inserta ORDER BY solo después de GROUP BY (si existe)
                if (ContainsClause("GROUP BY"))
                {
                    Sql += $" ORDER BY {sortBy} {safeSortDir}";
                }
                else
                {
                    Sql += $" ORDER BY {sortBy} {safeSortDir}";
                }
            }

            return this;
        }

        public QueryBuilder ApplyPagination(int page, int pageSize)
        {
            if (!Sql.Contains("ORDER BY", StringComparison.OrdinalIgnoreCase))
            {
                // Evita OFFSET sin ORDER BY (no permitido en SQL Server)
                Sql += " ORDER BY (SELECT NULL)";
            }

            Sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            Parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
            Parameters.Add(new SqlParameter("@PageSize", pageSize));

            return this;
        }

        #region Helpers

        private void InsertWhereCondition(string condition)
        {
            var groupByIndex = IndexOfClause("GROUP BY");
            var orderByIndex = IndexOfClause("ORDER BY");
            var insertPos = groupByIndex >= 0 ? groupByIndex : orderByIndex >= 0 ? orderByIndex : Sql.Length;

            if (Sql.Contains("WHERE", StringComparison.OrdinalIgnoreCase))
            {
                Sql = Sql.Insert(insertPos, $" AND {condition}");
            }
            else
            {
                Sql = Sql.Insert(insertPos, $" WHERE {condition}");
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
