using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class RoleMapper
    {
        public static Role? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));
            bool HasCol(string c) { try { r.GetOrdinal(c); return true; } catch { return false; } }

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            var permissions = new List<Permission>();
            if (HasCol(Col("Permissions")))
            {
                var raw = S(Col("Permissions"));
                if (!string.IsNullOrWhiteSpace(raw))
                {
                    foreach (var idStr in raw.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (int.TryParse(idStr, out int id) && Enum.IsDefined(typeof(Permission), id))
                            permissions.Add((Permission)id);
                    }
                }
            }

            return new Role(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                name: S(Col("Name")),
                description: S(Col("Description")),
                permissions: permissions,
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }


}

