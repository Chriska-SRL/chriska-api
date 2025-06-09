using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class UserMapper
    {
        public static User FromReader(SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var name = reader.GetString(reader.GetOrdinal("Name"));
            var username = reader.GetString(reader.GetOrdinal("Username"));
            var password = reader.GetString(reader.GetOrdinal("Password"));
            var isEnabledChar = reader.GetString(reader.GetOrdinal("IsEnabled"))[0];
            var isEnabled = isEnabledChar == 'T';
            var roleId = reader.GetInt32(reader.GetOrdinal("RoleId"));
            var role = new Role(roleId, "", "", new List<Permission>());

            return new User(id, name, username, password, isEnabled, role, new List<Request>());
        }
    }
}
