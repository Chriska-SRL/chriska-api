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
            Sql += Sql.Contains("WHERE", StringComparison.OrdinalIgnoreCase)
                ? " AND IsDeleted = 0"
                : " WHERE IsDeleted = 0";
            return this;
        }

        public QueryBuilder ApplyFilters(Dictionary<string, string>? filters)
        {
            if (filters == null) return this;

            foreach (var (key, value) in filters)
            {
                if (string.IsNullOrWhiteSpace(value)) continue;

                var paramName = $"@{key}";
                Sql += key.EndsWith("From", StringComparison.OrdinalIgnoreCase)
                    ? $" AND {key[..^4]} >= {paramName}"
                    : key.EndsWith("To", StringComparison.OrdinalIgnoreCase)
                    ? $" AND {key[..^2]} <= {paramName}"
                    : $" AND {key} LIKE {paramName}";

                Parameters.Add(new SqlParameter(paramName, $"%{value}%"));
            }
            return this;
        }

        public QueryBuilder ApplySorting(string? sortBy, string? sortDirection)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var safeSortDir = sortDirection?.ToUpperInvariant() == "DESC" ? "DESC" : "ASC";
                Sql += $" ORDER BY {sortBy} {safeSortDir}";
            }
            return this;
        }

        public QueryBuilder ApplyPagination(int page, int pageSize)
        {
            Sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            Parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
            Parameters.Add(new SqlParameter("@PageSize", pageSize));
            return this;
        }
    }

}
