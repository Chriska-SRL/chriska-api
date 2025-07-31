using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository;
using Repository.Logging;
using Repository.Mappers;

public class UserRepository : Repository<User, User.UpdatableData>, IUserRepository
{
    public UserRepository(string connectionString, AuditLogger auditLogger)
        : base(connectionString, auditLogger) { }

    public async Task<User> AddAsync(User user)
    {
        int newId = await ExecuteWriteWithAuditAsync(
            "INSERT INTO Users (Name, Username, Password, IsEnabled, NeedsPasswordChange, RoleId) OUTPUT INSERTED.Id VALUES (@Name, @Username, @Password, @IsEnabled, @NeedsPasswordChange, @RoleId)",
            user,
            AuditAction.Insert,
            cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@IsEnabled", user.IsEnabled ? "T" : "F");
                cmd.Parameters.AddWithValue("@NeedsPasswordChange", user.NeedsPasswordChange ? "T" : "F");
                cmd.Parameters.AddWithValue("@RoleId", user.Role.Id);
            },
            async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
        );

        user.Id = newId;
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        int rows = await ExecuteWriteWithAuditAsync(
            "UPDATE Users SET Name = @Name, Username = @Username, Password = @Password, IsEnabled = @IsEnabled, NeedsPasswordChange = @NeedsPasswordChange, RoleId = @RoleId WHERE Id = @Id",
            user,
            AuditAction.Update,
            cmd =>
            {
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@IsEnabled", user.IsEnabled ? "T" : "F");
                cmd.Parameters.AddWithValue("@NeedsPasswordChange", user.NeedsPasswordChange ? "T" : "F");
                cmd.Parameters.AddWithValue("@RoleId", user.Role.Id);
            }
        );

        if (rows == 0)
            throw new InvalidOperationException($"No se pudo actualizar el usuario con Id {user.Id}");

        return user;
    }

    public async Task<User> DeleteAsync(User user)
    {
        int rows = await ExecuteWriteWithAuditAsync(
            "UPDATE Users SET IsDeleted = 1 WHERE Id = @Id",
            user,
            AuditAction.Delete,
            cmd => cmd.Parameters.AddWithValue("@Id", user.Id)
        );

        if (rows == 0)
            throw new InvalidOperationException($"No se pudo eliminar el usuario con Id {user.Id}");

        return user;
    }

    public async Task<List<User>> GetAllAsync(QueryOptions options)
    {
        var allowedFilters = new[] { "Name", "Username", "RoleId", "IsEnabled" };
        return await ExecuteReadAsync(
            baseQuery: @"
                SELECT u.*, 
                       r.Id AS RoleId, r.Name AS RoleName, r.Description AS RoleDescription
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id",
            map: reader =>
            {
                var users = new List<User>();
                while (reader.Read())
                    users.Add(UserMapper.FromReader(reader, includePermissions: false));
                return users;
            },
            tableAlias: "u",
            options: options,
            allowedFilterColumns: allowedFilters
        );
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await ExecuteReadAsync(
            baseQuery: @"
            SELECT 
                u.Id, u.Name, u.Username, u.Password, u.IsEnabled, u.NeedsPasswordChange,
                r.Id AS RoleId, r.Name AS RoleName, r.Description AS RoleDescription,
                perms.Permissions
            FROM Users u
            INNER JOIN Roles r ON u.RoleId = r.Id
            OUTER APPLY (
                SELECT STRING_AGG(CONVERT(varchar(50), rp.PermissionId), ',') AS Permissions
                FROM Roles_Permissions rp
                WHERE rp.RoleId = r.Id
            ) AS perms
            WHERE u.Id = @Id",
            map: reader => reader.Read() ? UserMapper.FromReader(reader, includePermissions: true) : null,
            options: new QueryOptions(),
            tableAlias: "u",
            configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
        );
    }


    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await ExecuteReadAsync(
           baseQuery: @"
                SELECT 
                    u.Id, u.Name, u.Username, u.Password, u.IsEnabled, u.NeedsPasswordChange,
                    r.Id AS RoleId, r.Name AS RoleName, r.Description AS RoleDescription,
                    perms.Permissions
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id
                OUTER APPLY (
                    SELECT STRING_AGG(CONVERT(varchar(50), rp.PermissionId), ',') AS Permissions
                    FROM Roles_Permissions rp
                    WHERE rp.RoleId = r.Id
                ) AS perms
                WHERE u.Username = @Username",
            map: reader => reader.Read() ? UserMapper.FromReader(reader, includePermissions: true) : null,
            options: new QueryOptions(),
            tableAlias: "u",
            configureCommand: cmd => cmd.Parameters.AddWithValue("@Username", username)
        );
    }

}
