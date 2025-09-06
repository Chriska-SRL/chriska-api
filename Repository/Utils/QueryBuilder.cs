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
                    continue;

                var paramName = $"@p{Parameters.Count}";
                var columnSql = tableAlias is null ? baseKey : $"{tableAlias}.{baseKey}";

                bool isFrom = rawKey.EndsWith("From", StringComparison.OrdinalIgnoreCase);
                bool isTo = rawKey.EndsWith("To", StringComparison.OrdinalIgnoreCase);
                bool isIdEq = baseKey.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && !isFrom && !isTo;

                string condition =
                    isFrom ? $"{columnSql} >= {paramName}" :
                    isTo ? $"{columnSql} <= {paramName}" :
                    isIdEq ? $"{columnSql} = {paramName}" :
                             $"{columnSql} LIKE {paramName}";

                var paramValue =
                    (isFrom || isTo) ? value :
                    isIdEq ? value :
                    $"%{value}%";

                InsertWhereCondition(condition);
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
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            int fromIdx = IndexOfClause("FROM");
            if (fromIdx < 0)
            {
                if (!Sql.Contains("ORDER BY", StringComparison.OrdinalIgnoreCase))
                    Sql += " ORDER BY (SELECT NULL)";
                Sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                Parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
                Parameters.Add(new SqlParameter("@PageSize", pageSize));
                return this;
            }

            // Alias principal tras FROM <Tabla> [AS] <alias> ...
            string afterFrom = Sql.Substring(fromIdx + 4).TrimStart();
            var tk = afterFrom.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string alias = "";
            if (tk.Length >= 1)
            {
                int i = 0; // tk[0] tabla
                int j = i + 1;
                if (j < tk.Length && tk[j].Equals("AS", StringComparison.OrdinalIgnoreCase)) j++;
                if (j < tk.Length && tk[j].StartsWith("WITH", StringComparison.OrdinalIgnoreCase)) j++; // WITH(NOLOCK)...
                string[] kw = { "WHERE", "JOIN", "LEFT", "RIGHT", "FULL", "INNER", "OUTER", "GROUP", "ORDER", "CROSS", "APPLY", "UNION" };
                if (j < tk.Length && !kw.Any(k => tk[j].Equals(k, StringComparison.OrdinalIgnoreCase)))
                    alias = tk[j].Trim().TrimEnd(',');
            }

            // Caso simple: sin alias y sin JOIN → OFFSET/FETCH directo
            if (string.IsNullOrWhiteSpace(alias) && !Sql.Contains("JOIN", StringComparison.OrdinalIgnoreCase))
            {
                if (!Sql.Contains("ORDER BY", StringComparison.OrdinalIgnoreCase))
                    Sql += " ORDER BY Id ASC";
                Sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                Parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
                Parameters.Add(new SqlParameter("@PageSize", pageSize));
                return this;
            }

            // Cortes de cláusulas
            int orderIdx = Sql.LastIndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
            int groupIdx = IndexOfClause("GROUP BY");
            int whereIdx = IndexOfClause("WHERE");
            int endIdx = orderIdx >= 0 ? orderIdx : Sql.Length;

            string selectClause = Sql.Substring(0, fromIdx).Trim();

            int firstTailIdx = endIdx;
            if (whereIdx >= 0 && whereIdx > fromIdx) firstTailIdx = Math.Min(firstTailIdx, whereIdx);
            if (groupIdx >= 0 && groupIdx > fromIdx) firstTailIdx = Math.Min(firstTailIdx, groupIdx);
            string fromAndJoins = Sql.Substring(fromIdx, firstTailIdx - fromIdx).TrimEnd();

            string whereOnly = "";
            if (whereIdx >= 0 && whereIdx < endIdx)
            {
                int whereEnd = endIdx;
                if (groupIdx >= 0 && groupIdx > whereIdx) whereEnd = Math.Min(whereEnd, groupIdx);
                whereOnly = Sql.Substring(whereIdx, whereEnd - whereIdx).Trim(); // "WHERE ..."
            }

            string groupAndHaving = "";
            if (groupIdx >= 0 && groupIdx < endIdx)
                groupAndHaving = Sql.Substring(groupIdx, endIdx - groupIdx).TrimEnd();

            string orderExpr = orderIdx >= 0
                ? Sql.Substring(orderIdx + "ORDER BY".Length).Trim()
                : (!string.IsNullOrWhiteSpace(alias) ? $"{alias}.Id ASC" : "Id ASC");

            // CTE __r con FROM/JOINS/WHERE (para que existan c/u/orq en el ORDER BY)
            // Luego __u colapsa por Id y __page densifica y recorta por rango.
            string idCol = string.IsNullOrWhiteSpace(alias) ? "Id" : $"{alias}.Id";
            string cte = $@"
WITH __r AS (
    SELECT {idCol} AS Id,
           ROW_NUMBER() OVER (ORDER BY {orderExpr}) AS __rn
    {Environment.NewLine}{fromAndJoins}
    {(string.IsNullOrWhiteSpace(whereOnly) ? "" : Environment.NewLine + whereOnly)}
),
__u AS (
    SELECT Id, MIN(__rn) AS __rn
    FROM __r
    GROUP BY Id
),
__page AS (
    SELECT Id
    FROM (
        SELECT Id, ROW_NUMBER() OVER (ORDER BY __rn) AS __drn
        FROM __u
    ) q
    WHERE __drn BETWEEN @__From AND @__To
)";

            // SELECT final: reusa FROM/JOINS + WHERE + GROUP/HAVING y se limita con __page
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(selectClause);
            sb.AppendLine(fromAndJoins);
            sb.AppendLine($"JOIN __page __pg ON __pg.Id = {idCol}");
            if (!string.IsNullOrWhiteSpace(whereOnly)) sb.AppendLine(whereOnly);
            if (!string.IsNullOrWhiteSpace(groupAndHaving)) sb.AppendLine(groupAndHaving);
            sb.AppendLine($"ORDER BY {orderExpr}");

            Sql = cte + Environment.NewLine + sb.ToString();

            int offset = (page - 1) * pageSize;
            Parameters.Add(new SqlParameter("@__From", offset + 1));
            Parameters.Add(new SqlParameter("@__To", offset + pageSize));
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

        public static string BuildBulkInsertQuery(string tableName, int count, params string[] columns)
        {
            var values = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var placeholders = columns.Select(c => $"@{c}{i}");
                values.Add($"({string.Join(", ", placeholders)})");
            }

            return $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES {string.Join(", ", values)}";
        }

        #endregion
    }
}
