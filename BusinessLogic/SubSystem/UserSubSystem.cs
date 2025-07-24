using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Común;
using BusinessLogic.DTOs;

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

        public async Task<UserResponse> AddUserAsync(AddUserRequest request)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId)
                       ?? throw new ArgumentException("Rol no encontrado.", nameof(request.RoleId));

            var newUser = UserMapper.ToDomain(request);
            newUser.Role = role;
            User.ValidatePassword(request.Password);
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            newUser.Validate();

            if (await _userRepository.GetByUsernameAsync(newUser.Username) != null)
                throw new ArgumentException("Ya existe un usuario con ese username.", nameof(newUser.Username));

            var added = await _userRepository.AddAsync(newUser);
            return UserMapper.ToResponse(added);
        }


        public async Task<string> ResetPasswordAsync(int userId, string? newPassword = null)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new InvalidOperationException("Usuario no encontrado.");

            if (string.IsNullOrEmpty(newPassword))
            {
                newPassword = PasswordGenerator.Generate();
                user.NeedsPasswordChange = true;
            }
            else
            {
                user.NeedsPasswordChange = false;
            }
            User.ValidatePassword(newPassword);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _userRepository.UpdateAsync(user);

            return newPassword;
        }
        public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.Id)
                                ?? throw new ArgumentException("Usuario no encontrado.", nameof(request.Id));

            var role = await _roleRepository.GetByIdAsync(request.RoleId)
                       ?? throw new ArgumentException("Rol no encontrado.", nameof(request.RoleId));

            if (existingUser.Username != request.Username && await _userRepository.GetByUsernameAsync(request.Username) != null)
                throw new ArgumentException("Ya existe un usuario con ese username.", nameof(request.Username));

            var updatedData = UserMapper.ToUpdatableData(request);
            updatedData.Role = role;
            existingUser.Update(updatedData);

            var updated = await _userRepository.UpdateAsync(existingUser);
            return UserMapper.ToResponse(updated);
        }
        public async Task<UserResponse> DeleteUserAsync(DeleteRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.Id)
                        ?? throw new InvalidOperationException("Usuario no encontrado.");

            var auditInfo = AuditMapper.ToDomain(request.AuditInfo);
            user.SetDeletedAudit(auditInfo);

            await _userRepository.DeleteAsync(user);
            return UserMapper.ToResponse(user);
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                       ?? throw new ArgumentException("Usuario no encontrado.", nameof(id));

            return UserMapper.ToResponse(user);
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(QueryOptions options)
        {
            var users = await _userRepository.GetAllAsync(options);
            return users.Select(UserMapper.ToResponse).ToList();
        }
    }
}
