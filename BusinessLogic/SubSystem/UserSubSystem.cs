using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Interface;
using BusinessLogic.Repository;

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

        public async Task<(UserResponse, string Password)> AddUserAsync(AddUserRequest request)
        {
            if (await _userRepository.GetByUsernameAsync(request.Username) != null)
                throw new ArgumentException("Ya existe un usuario con ese nombre de usuario.", nameof(request.Username));

            var role = await _roleRepository.GetByIdAsync(request.RoleId)
             ?? throw new ArgumentException("No se encontró el rol asociado.", nameof(request.RoleId));

            var newUser = UserMapper.ToDomain(request);
            newUser.Validate();
            string password = request.Password ?? PasswordGenerator.Generate();
            newUser.SetPassword(password);

            var added = await _userRepository.AddAsync(newUser);
            return (UserMapper.ToResponse(added), Password: password);
        }

        public async Task<string> ResetPasswordAsync(int userId, string? newPassword = null)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ArgumentException("No se encontró el usuario seleccionado.", nameof(userId));

            string password = newPassword;
            if (string.IsNullOrWhiteSpace(password))
            {
                password = PasswordGenerator.Generate();
                user.NeedsPasswordChange = true;
            }else
            {
                user.NeedsPasswordChange = false;
            }

            user.SetPassword(password);
            await _userRepository.UpdateAsync(user);
            return password;
        }

        public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el usuario seleccionado.", nameof(request.Id));

            if (existingUser.Username != request.Username &&
                await _userRepository.GetByUsernameAsync(request.Username)!= null)
                throw new ArgumentException("Ya existe un usuario con ese nombre de usuario.", nameof(request.Username));

            if (existingUser.Role.Id != request.RoleId) { 
            var role = await _roleRepository.GetByIdAsync(request.RoleId)
                ?? throw new ArgumentException("No se encontró el rol asociado.", nameof(request.RoleId));
            }

            var updatedData = UserMapper.ToUpdatableData(request);
            existingUser.Update(updatedData);

            var updated = await _userRepository.UpdateAsync(existingUser);
            return UserMapper.ToResponse(updated);
        }

        public async Task DeleteUserAsync(DeleteRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.Id)
                ?? throw new ArgumentException("No se encontró el usuario seleccionado.", nameof(request.Id));

            user.MarkAsDeleted(request.getUserId(), request.Location);
            await _userRepository.DeleteAsync(user);
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new ArgumentException("No se encontró el usuario seleccionado.", nameof(id));

            return UserMapper.ToResponse(user);
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(QueryOptions options)
        {
            var users = await _userRepository.GetAllAsync(options);
            return users.Select(u => UserMapper.ToResponse(u)).ToList();
        }

        public async Task<UserResponse> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username)
                ?? throw new ArgumentException("No se encontró un usuario con ese nombre de usuario.", nameof(username));

            return UserMapper.ToResponse(user);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username) != null;
        }

    }
}
