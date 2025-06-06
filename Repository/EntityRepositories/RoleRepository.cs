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
                using var command = new SqlCommand(@"INSERT INTO Roles (Name) OUTPUT INSERTED.Id VALUES (@Name)", connection);

                command.Parameters.AddWithValue("@Name", role.Name);

                connection.Open();
                int roleId = (int)command.ExecuteScalar();

                InsertPermissions(connection, roleId, role.Permissions);

                return new Role(roleId, role.Name, role.Permissions);
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

                var role = GetById(id);
                
                if (role == null) 
                {
                    _logger.LogWarning($"No se encontró el rol con ID {id} para eliminar.");
                    return null;
                }

                using (var deletePermissions = new SqlCommand("DELETE FROM Roles_Permissions WHERE RoleId = @Id", connection))
                {
                    deletePermissions.Parameters.AddWithValue("@Id", id);
                    deletePermissions.ExecuteNonQuery();
                }

                using (var deleteRole = new SqlCommand("DELETE FROM Roles WHERE Id = @Id", connection))
                {
                    deleteRole.Parameters.AddWithValue("@Id", id);
                    deleteRole.ExecuteNonQuery();
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

                using (var command = new SqlCommand("SELECT Id, Name FROM Roles", connection))
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
                Role role;

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name FROM Roles WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    role = RoleMapper.FromReader(reader);
                }

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
                Role role;

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name FROM Roles WHERE Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    role = RoleMapper.FromReader(reader);
                }

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

                using (var updateCommand = new SqlCommand("UPDATE Roles SET Name = @Name WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", role.Name);
                    updateCommand.Parameters.AddWithValue("@Id", role.Id);
                    updateCommand.ExecuteNonQuery();
                }

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
            }
            return permissions;
        }

        private void InsertPermissions(SqlConnection connection, int roleId, List<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                using var command = new SqlCommand("INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (@RoleId, @PermissionId)", connection);
                command.Parameters.AddWithValue("@RoleId", roleId);
                command.Parameters.AddWithValue("@PermissionId", (int)permission);
                command.ExecuteNonQuery();
            }
        }

    }
}
