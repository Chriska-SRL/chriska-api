using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Común;

namespace BusinessLogic.SubSystem
{
    public class UserSubSystem
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserSubSystem(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public UserResponse AddUser(AddUserRequest request)
        {
            var role = _roleRepository.GetById(request.RoleId)
                       ?? throw new ArgumentException("Rol no encontrado.", nameof(request.RoleId));

            var newUser = UserMapper.ToDomain(request);
            newUser.Role = role;
            User.ValidatePassword(request.Password);
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            newUser.Validate();

            if (_userRepository.GetByUsername(newUser.Username) != null) 
                        throw new ArgumentException("Ya existe un usuario con ese username.", nameof(newUser.Username));

            var added = _userRepository.Add(newUser);
            return UserMapper.ToResponse(added);
        }

        public string ResetPassword(int userId, string? newPassword = null)
        {
            var user = _userRepository.GetById(userId)
                       ?? throw new InvalidOperationException("Usuario no encontrado.");

            if (string.IsNullOrEmpty(newPassword))
            {
                newPassword = PasswordGenerator.Generate();
                user.needsPasswordChange = true;
            }
            User.ValidatePassword(newPassword);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _userRepository.Update(user);

            return newPassword;
        }

        public UserResponse UpdateUser(UpdateUserRequest request)
        {
            var existingUser = _userRepository.GetById(request.Id)
                                ?? throw new ArgumentException("Usuario no encontrado.", nameof(request.Id));

            var role = _roleRepository.GetById(request.RoleId)
                       ?? throw new ArgumentException("Rol no encontrado.", nameof(request.RoleId));

            if (existingUser.Username != request.Username && _userRepository.GetByUsername(request.Username) != null)
                throw new ArgumentException("Ya existe un usuario con ese username.", nameof(request.Username));

            var updatedData = UserMapper.ToUpdatableData(request);
            updatedData.Role = role;
            existingUser.Update(updatedData);

            var updated = _userRepository.Update(existingUser);
            return UserMapper.ToResponse(updated);
        }

        public UserResponse DeleteUser(int id)
        {
            var deleted = _userRepository.Delete(id)
                          ?? throw new ArgumentException("Usuario no encontrado.", nameof(id));

            //TODO: Implementar control de integridad referencial:

            return UserMapper.ToResponse(deleted);
        }

        public UserResponse GetUserById(int id)
        {
            var user = _userRepository.GetById(id)
                       ?? throw new ArgumentException("Usuario no encontrado.", nameof(id));

            return UserMapper.ToResponse(user);
        }

        public List<UserResponse> GetAllUsers()
        {
            return _userRepository.GetAll()
                                  .Select(UserMapper.ToResponse)
                                  .ToList();
        }
    }
}
