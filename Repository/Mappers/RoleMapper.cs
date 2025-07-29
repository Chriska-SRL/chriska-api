using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class RoleMapper
    {
        public static Role FromReader(SqlDataReader reader)
        {
            var permissions = new List<Permission>();

            if (ColumnExists(reader, "Permissions"))
            {
                var permissionsCsv = reader["Permissions"] as string;
                if (!string.IsNullOrEmpty(permissionsCsv))
                {
                    permissions = permissionsCsv
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(id => Enum.Parse<Permission>(id))
                        .ToList();
                }
            }

            return new Role(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                permissions: permissions,
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }

        private static bool ColumnExists(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }


    }
}
