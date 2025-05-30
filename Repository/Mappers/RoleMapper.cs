using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class RoleMapper
    {
        public static Role FromReader(SqlDataReader reader)
        {
            return new Role(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                permissions: new List<Permission>()
            );
        }
    }
}
