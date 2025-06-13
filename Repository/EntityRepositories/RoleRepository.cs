using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class RoleRepository : Repository<RoleRepository>, IRoleRepository
    {
        public RoleRepository(string connectionString, ILogger<RoleRepository> logger) : base(connectionString, logger)
        {
        }

        public Role Add(Role role)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"INSERT INTO Roles (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)", connection);

                command.Parameters.AddWithValue("@Name", role.Name);
                command.Parameters.AddWithValue("@Description", role.Description);

                connection.Open();
                int roleId = (int)command.ExecuteScalar();

                InsertPermissions(connection, roleId, role.Permissions);

                return new Role(roleId, role.Name, role.Description, role.Permissions);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Role? Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Intentando eliminar el rol con ID {id}.");
                using var connection = CreateConnection();
                connection.Open();

                var role = GetRoleWithoutPermissions(id, connection);
                if (role == null)
                {
                    _logger.LogWarning($"No se encontró el rol con ID {id} para eliminar.");
                    return null;
                }

                role.Permissions = GetPermissionsForRole(role.Id, connection);

                using (var deletePermissions = new SqlCommand("DELETE FROM Roles_Permissions WHERE RoleId = @Id", connection))
                {
                    deletePermissions.Parameters.AddWithValue("@Id", id);
                    deletePermissions.ExecuteNonQuery();
                }

                using (var deleteRole = new SqlCommand("DELETE FROM Roles WHERE Id = @Id", connection))
                {
                    deleteRole.Parameters.AddWithValue("@Id", id);
                    int deleted = deleteRole.ExecuteNonQuery();
                    if (deleted == 0)
                        throw new InvalidOperationException($"El rol con ID {id} no fue eliminado.");
                }

                _logger.LogInformation($"Rol con ID {id} eliminado correctamente.");
                return role;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }


        public List<Role> GetAll()
        {
            try
            {
                var roles = new List<Role>();

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name, Description FROM Roles", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(RoleMapper.FromReader(reader));
                    }
                }

                foreach (var role in roles)
                {
                    role.Permissions = GetPermissionsForRole(role.Id, connection);
                }

                return roles;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Role? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var role = GetRoleWithoutPermissions(id, connection);
                if (role == null) return null;

                role.Permissions = GetPermissionsForRole(role.Id, connection);
                return role;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Role? GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var role = GetRoleWithoutPermissions(name, connection);
                if (role == null) return null;

                role.Permissions = GetPermissionsForRole(role.Id, connection);
                return role;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Role Update(Role role)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                int rowsAffected;
                using (var updateCommand = new SqlCommand("UPDATE Roles SET Name = @Name, Description = @Description WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", role.Name);
                    updateCommand.Parameters.AddWithValue("@Description", role.Description);
                    updateCommand.Parameters.AddWithValue("@Id", role.Id);
                    rowsAffected = updateCommand.ExecuteNonQuery();
                }

                if (rowsAffected == 0)
                    throw new InvalidOperationException($"No se encontró el rol con ID {role.Id} para actualizar.");

                using (var deletePermissions = new SqlCommand("DELETE FROM Roles_Permissions WHERE RoleId = @RoleId", connection))
                {
                    deletePermissions.Parameters.AddWithValue("@RoleId", role.Id);
                    deletePermissions.ExecuteNonQuery();
                }

                InsertPermissions(connection, role.Id, role.Permissions);
                return role;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        private Role? GetRoleWithoutPermissions(int id, SqlConnection connection)
        {
            using var command = new SqlCommand("SELECT Id, Name, Description FROM Roles WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read() ? RoleMapper.FromReader(reader) : null;
        }

        private Role? GetRoleWithoutPermissions(string name, SqlConnection connection)
        {
            using var command = new SqlCommand("SELECT Id, Name, Description FROM Roles WHERE Name = @Name", connection);
            command.Parameters.AddWithValue("@Name", name);
            using var reader = command.ExecuteReader();
            return reader.Read() ? RoleMapper.FromReader(reader) : null;
        }

        private List<Permission> GetPermissionsForRole(int roleId, SqlConnection connection)
        {
            var permissions = new List<Permission>();
            using var command = new SqlCommand("SELECT PermissionId FROM Roles_Permissions WHERE RoleId = @RoleId", connection);
            command.Parameters.AddWithValue("@RoleId", roleId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int permissionId = reader.GetInt32(reader.GetOrdinal("PermissionId"));
                if (Enum.IsDefined(typeof(Permission), permissionId))
                {
                    permissions.Add((Permission)permissionId);
                }
                else
                {
                    throw new InvalidOperationException($"PermissionId inválido encontrado en la base de datos: {permissionId}");
                }
            }
            return permissions;
        }

        private void InsertPermissions(SqlConnection connection, int roleId, List<Permission> permissions)
        {
            if (permissions == null)
                throw new ArgumentNullException(nameof(permissions), "Los permisos no pueden ser nulos.");

            foreach (var permission in permissions)
            {
                if (!Enum.IsDefined(typeof(Permission), (int)permission))
                    throw new InvalidOperationException($"Permiso inválido: {(int)permission}");

                using var command = new SqlCommand("INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (@RoleId, @PermissionId)", connection);
                command.Parameters.AddWithValue("@RoleId", roleId);
                command.Parameters.AddWithValue("@PermissionId", (int)permission);
                command.ExecuteNonQuery();
            }
        }

    }
}
