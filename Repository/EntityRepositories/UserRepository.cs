using BusinessLogic.Común;
using BusinessLogic.Dominio;
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
                cmd.Parameters.AddWithValue("@IsEnabled", user.isEnabled ? "T" : "F");
                cmd.Parameters.AddWithValue("@NeedsPasswordChange", user.needsPasswordChange ? "T" : "F");
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
                cmd.Parameters.AddWithValue("@IsEnabled", user.isEnabled ? "T" : "F");
                cmd.Parameters.AddWithValue("@NeedsPasswordChange", user.needsPasswordChange ? "T" : "F");
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
            options: options
        );
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await ExecuteReadAsync(
            baseQuery: @"
                SELECT u.*, 
                       r.Id AS RoleId, r.Name AS RoleName, r.Description AS RoleDescription,
                       STRING_AGG(rp.PermissionId, ',') AS Permissions
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id
                LEFT JOIN Roles_Permissions rp ON r.Id = rp.RoleId
                WHERE u.Id = @Id
                GROUP BY u.Id, u.Name, u.Username, u.Password, u.IsEnabled, u.NeedsPasswordChange, u.RoleId,
                         r.Id, r.Name, r.Description,
                         u.CreatedAt, u.CreatedBy, u.CreatedLocation,
                         u.UpdatedAt, u.UpdatedBy, u.UpdatedLocation,
                         u.DeletedAt, u.DeletedBy, u.DeletedLocation, u.IsDeleted",
            map: reader => reader.Read() ? UserMapper.FromReader(reader, includePermissions: true) : null,
            options: new QueryOptions(),
            configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
        );
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await ExecuteReadAsync(
            baseQuery: @"
                SELECT u.*, 
                       r.Id AS RoleId, r.Name AS RoleName, r.Description AS RoleDescription,
                       STRING_AGG(rp.PermissionId, ',') AS Permissions
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id
                LEFT JOIN Roles_Permissions rp ON r.Id = rp.RoleId
                WHERE u.Username = @Username
                GROUP BY u.Id, u.Name, u.Username, u.Password, u.IsEnabled, u.NeedsPasswordChange, u.RoleId,
                         r.Id, r.Name, r.Description,
                         u.CreatedAt, u.CreatedBy, u.CreatedLocation,
                         u.UpdatedAt, u.UpdatedBy, u.UpdatedLocation,
                         u.DeletedAt, u.DeletedBy, u.DeletedLocation, u.IsDeleted",
            map: reader => reader.Read() ? UserMapper.FromReader(reader, includePermissions: true) : null,
            options: new QueryOptions(),
            configureCommand: cmd => cmd.Parameters.AddWithValue("@Username", username)
        );
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        using var cmd = new SqlCommand("SELECT 1 FROM Users WHERE Username = @Username AND IsDeleted = 0", connection);
        cmd.Parameters.AddWithValue("@Username", username);

        var result = await cmd.ExecuteScalarAsync();
        return result != null;
    }
}
