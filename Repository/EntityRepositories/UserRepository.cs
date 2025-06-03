using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class UserRepository : Repository<UserRepository>, IUserRepository
    {
        public UserRepository(string connectionString, ILogger<UserRepository> logger) : base(connectionString, logger)
        {
        }

        public User Add(User user)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO Users (Name, Username, Password, IsEnabled, RoleId)
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Username, @Password, @IsEnabled, @RoleId)", connection);

                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@IsEnabled", user.isEnabled ? 'T' : 'F');
                command.Parameters.AddWithValue("@RoleId", user.Role.Id);

                connection.Open();
                int userId = (int)command.ExecuteScalar();

                return new User(userId, user.Name, user.Username, user.Password, user.isEnabled, user.Role, new List<Request>());
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

        public User? Delete(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                var user = GetById(id);
                if (user == null) return null;

                using var deleteCommand = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();

                return user;
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

        public List<User> GetAll()
        {
            try
            {
                var users = new List<User>();
                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name, Username, Password, IsEnabled, RoleId FROM Users", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(UserMapper.FromReader(reader));
                    }
                }

                foreach (var user in users)
                {
                    user.Role = GetRoleWithPermissions(user.Role.Id, connection);
                }

                return users;
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


        public User? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                User? user;

                using (var command = new SqlCommand("SELECT Id, Name, Username, Password, IsEnabled, RoleId FROM Users WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    user = UserMapper.FromReader(reader);
                }

                user.Role = GetRoleWithPermissions(user.Role.Id, connection);

                return user;
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


        public User? GetByUsername(string username)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                User? user;

                using (var command = new SqlCommand("SELECT Id, Name, Username, Password, IsEnabled, RoleId FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    user = UserMapper.FromReader(reader);
                }

                user.Role = GetRoleWithPermissions(user.Role.Id, connection);

                return user;
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


        public bool ExistsByUsername(string username)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                int count = (int)command.ExecuteScalar();

                return count > 0;
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

        public User Update(User user)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand(@"
                    UPDATE Users 
                    SET Name = @Name, Username = @Username, Password = @Password, IsEnabled = @IsEnabled, RoleId = @RoleId 
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@IsEnabled", user.isEnabled ? 'T' : 'F');
                command.Parameters.AddWithValue("@RoleId", user.Role.Id);

                command.ExecuteNonQuery();

                return user;
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

        private Role GetRoleWithPermissions(int roleId, SqlConnection connection)
        {
            Role? role = null;

            using (var command = new SqlCommand("SELECT Id, Name FROM Roles WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", roleId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    role = new Role(
                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        name: reader.GetString(reader.GetOrdinal("Name")),
                        permissions: new List<Permission>()
                    );
                }
            }

            if (role == null) throw new Exception("Rol no encontrado.");

            using (var permCommand = new SqlCommand("SELECT PermissionId FROM Roles_Permissions WHERE RoleId = @RoleId", connection))
            {
                permCommand.Parameters.AddWithValue("@RoleId", role.Id);
                using var permReader = permCommand.ExecuteReader();
                while (permReader.Read())
                {
                    int permId = permReader.GetInt32(permReader.GetOrdinal("PermissionId"));
                    if (Enum.IsDefined(typeof(Permission), permId))
                    {
                        role.Permissions.Add((Permission)permId);
                    }
                }
            }

            return role;
        }
    }
}
