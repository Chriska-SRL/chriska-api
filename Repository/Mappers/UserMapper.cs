using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Repository.Mappers
{
    public static class UserMapper
    {
        public static User? FromReader(SqlDataReader r, string? prefix = null, bool includePermissions = false)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            var role = new Role(
                id: r.GetInt32(r.GetOrdinal(Col("RoleId"))),
                name: S(Col("RoleName")),
                description: S(Col("RoleDescription")),
                permissions: includePermissions ? ParsePermissions(r, Col("Permissions")) : new List<Permission>(),
                auditInfo: new BusinessLogic.Common.AuditInfo()
            );

            return new User(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                name: S(Col("Name")),
                username: S(Col("Username")),
                password: S(Col("Password")),
                isEnabled: S(Col("IsEnabled")) == "T",
                needsPasswordChange: S(Col("NeedsPasswordChange")) == "T",
                role: role,
                auditInfo: AuditInfoMapper.FromReader(r)
            );
        }

        private static List<Permission> ParsePermissions(SqlDataReader r, string colName)
        {
            var permissions = new List<Permission>();

            if (!r.IsDBNull(r.GetOrdinal(colName)))
            {
                var raw = r.GetString(r.GetOrdinal(colName));
                foreach (var idStr in raw.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(idStr, out int id) && Enum.IsDefined(typeof(Permission), id))
                        permissions.Add((Permission)id);
                }
            }

            return permissions;
        }

        public static User? FromReaderForRole(DbDataReader r, string? prefix = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            try
            {
                var o = r.GetOrdinal(Col("UserId"));
                if (r.IsDBNull(o)) return null;
            }
            catch
            {
                return null;
            }

            return new User(
                id: r.GetInt32(r.GetOrdinal(Col("UserId"))),
                name: S(Col("UserName")),
                username: S(Col("UserUsername")),
                password: null,
                isEnabled: S(Col("UserIsEnabled")) == "T",
                needsPasswordChange: S(Col("UserNeedsPasswordChange")) == "T",
                role: null,
                auditInfo: null
            );
        }
    }
}
