using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;
using Repository.Mappers;
using System.Data.Common;

public static class UserMapper
{
    public static User FromReader(SqlDataReader reader, bool includePermissions)
    {
        var role = new Role(
            id: reader.GetInt32(reader.GetOrdinal("RoleId")),
            name: reader.GetString(reader.GetOrdinal("RoleName")),
            description: reader.GetString(reader.GetOrdinal("RoleDescription")),
            permissions: includePermissions ? ParsePermissions(reader) : new List<Permission>(),
            auditInfo: new BusinessLogic.Common.AuditInfo()
        );

        var user = new User(
            id: reader.GetInt32(reader.GetOrdinal("Id")),
            name: reader.GetString(reader.GetOrdinal("Name")),
            username: reader.GetString(reader.GetOrdinal("Username")),
            password: reader.GetString(reader.GetOrdinal("Password")),
            isEnabled: reader.GetString(reader.GetOrdinal("IsEnabled")) == "T",
            needsPasswordChange: reader.GetString(reader.GetOrdinal("NeedsPasswordChange")) == "T",
            role: role,
            auditInfo: AuditInfoMapper.FromReader(reader)
        );

        return user;
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

    public static User FromReaderForRole(DbDataReader reader)
    {
        return new User(
            id: reader.GetInt32(reader.GetOrdinal("UserId")),
            name: reader.GetString(reader.GetOrdinal("UserName")),
            username: reader.GetString(reader.GetOrdinal("UserUsername")),
            password: null,
            isEnabled: reader.GetString(reader.GetOrdinal("UserIsEnabled")) == "T",
            needsPasswordChange: reader.GetString(reader.GetOrdinal("UserNeedsPasswordChange")) == "T",
            role: null,
            auditInfo: null
        );
    }

}
