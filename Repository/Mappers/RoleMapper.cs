using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class RoleMapper
    {
        public static Role FromReader(SqlDataReader reader)
        {
            var permissions = new List<Permission>();

            if (!reader.IsDBNull(reader.GetOrdinal("Permissions")))
            {
                var rawPermissions = reader.GetString(reader.GetOrdinal("Permissions"));
                foreach (var idStr in rawPermissions.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(idStr, out int id) && Enum.IsDefined(typeof(Permission), id))
                        permissions.Add((Permission)id);
                }
            }

            return new Role(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                permissions: permissions
            );
        }
    }
}
