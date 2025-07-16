using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using BusinessLogic.Común;
using Microsoft.Data.SqlClient;
using System.Text;

namespace Repository.EntityRepositories
{
    public class RoleRepository : Repository<Role, Role.UpdatableData>, IRoleRepository
    {
        public RoleRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Role> AddAsync(Role role)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Roles (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)",
                role,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", role.Name);
                    cmd.Parameters.AddWithValue("@Description", role.Description);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            await SavePermissionsAsync(newId, role.Permissions);

            return new Role(newId, role.Name, role.Description, role.Permissions, role.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Role> UpdateAsync(Role role)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Roles SET Name = @Name, Description = @Description WHERE Id = @Id",
                role,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", role.Id);
                    cmd.Parameters.AddWithValue("@Name", role.Name);
                    cmd.Parameters.AddWithValue("@Description", role.Description);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el rol con Id {role.Id}");

            await SavePermissionsAsync(role.Id, role.Permissions);

            return role;
        }

        #endregion

        #region Delete

        public async Task<Role> DeleteAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role), "El rol no puede ser nulo.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Roles SET IsDeleted = 1 WHERE Id = @Id",
                role,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", role.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el rol con Id {role.Id}");

            return role;
        }

        #endregion

        #region GetAll

        public async Task<List<Role>> GetAllAsync(QueryOptions options)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                    SELECT r.*, 
                           STRING_AGG(rp.PermissionId, ',') AS Permissions
                    FROM Roles r
                    LEFT JOIN Roles_Permissions rp ON r.Id = rp.RoleId
                    GROUP BY r.Id, r.Name, r.Description, r.CreatedAt, r.CreatedBy, r.CreatedLocation,
                             r.UpdatedAt, r.UpdatedBy, r.UpdatedLocation,
                             r.DeletedAt, r.DeletedBy, r.DeletedLocation, r.IsDeleted",
                map: reader =>
                {
                    var roles = new List<Role>();
                    while (reader.Read())
                    {
                        roles.Add(RoleMapper.FromReader(reader));
                    }
                    return roles;
                },
                options: options
            );
        }

        #endregion

        #region GetById

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                    SELECT r.*, 
                           STRING_AGG(rp.PermissionId, ',') AS Permissions
                    FROM Roles r
                    LEFT JOIN Roles_Permissions rp ON r.Id = rp.RoleId
                    WHERE r.Id = @Id
                    GROUP BY r.Id, r.Name, r.Description, r.CreatedAt, r.CreatedBy, r.CreatedLocation,
                             r.UpdatedAt, r.UpdatedBy, r.UpdatedLocation,
                             r.DeletedAt, r.DeletedBy, r.DeletedLocation, r.IsDeleted",
                map: reader =>
                {
                    if (reader.Read())
                        return RoleMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }

        #endregion

        #region Private Helpers

        private async Task SavePermissionsAsync(int roleId, List<Permission> permissions)
        {
            await using var connection = CreateConnection();
            await connection.OpenAsync();

            // Elimina permisos actuales
            using var deleteCmd = new SqlCommand("DELETE FROM Roles_Permissions WHERE RoleId = @RoleId", connection);
            deleteCmd.Parameters.AddWithValue("@RoleId", roleId);
            await deleteCmd.ExecuteNonQueryAsync();

            if (permissions.Count == 0)
                return; // No hay permisos para insertar

            // Construye un solo comando SQL para el batch insert
            var sb = new StringBuilder();
            var parameters = new List<SqlParameter>();
            int index = 0;

            foreach (var permission in permissions)
            {
                var paramName = $"@PermissionId{index}";
                sb.AppendLine($"INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (@RoleId, {paramName});");
                parameters.Add(new SqlParameter(paramName, (int)permission));
                index++;
            }

            using var insertCmd = new SqlCommand(sb.ToString(), connection);
            insertCmd.Parameters.AddWithValue("@RoleId", roleId);
            insertCmd.Parameters.AddRange(parameters.ToArray());

            await insertCmd.ExecuteNonQueryAsync();
        }
        #endregion
    }
}
