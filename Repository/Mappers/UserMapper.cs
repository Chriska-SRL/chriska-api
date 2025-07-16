using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using Repository.Mappers;

public static class UserMapper
{
    public static User FromReader(SqlDataReader reader, bool includePermissions)
    {
        var role = new Role
        {
            Id = reader.GetInt32(reader.GetOrdinal("RoleId")),
            Name = reader.GetString(reader.GetOrdinal("RoleName")),
            Description = reader.GetString(reader.GetOrdinal("RoleDescription")),
            Permissions = includePermissions ? ParsePermissions(reader) : new List<Permission>()
        };

        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Username = reader.GetString(reader.GetOrdinal("Username")),
            Password = reader.GetString(reader.GetOrdinal("Password")),
            isEnabled = reader.GetString(reader.GetOrdinal("IsEnabled")) == "T",
            needsPasswordChange = reader.GetString(reader.GetOrdinal("NeedsPasswordChange")) == "T",
            Role = role,
            AuditInfo = AuditInfoMapper.FromReader(reader)
        };
    }

    private static List<Permission> ParsePermissions(SqlDataReader reader)
    {
        var permissions = new List<Permission>();

        if (!reader.IsDBNull(reader.GetOrdinal("Permissions")))
        {
            var raw = reader.GetString(reader.GetOrdinal("Permissions"));
            foreach (var idStr in raw.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(idStr, out int id) && Enum.IsDefined(typeof(Permission), id))
                    permissions.Add((Permission)id);
            }
        }

        return permissions;
    }
}
