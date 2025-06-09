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

        public UserSubSystem(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponse AddUser(AddUserRequest request)
        {
            var role = _roleRepository.GetById(request.RoleId)
                       ?? throw new InvalidOperationException("Rol no encontrado.");

            var newUser = UserMapper.ToDomain(request);
            newUser.Role = role;
            newUser.Password = PasswordGenerator.Generate();
            newUser.Validate();

            var added = _userRepository.Add(newUser);
            return UserMapper.ToResponse(added);
        }

        public UserResponse UpdateUser(UpdateUserRequest request)
        {
            var existingUser = _userRepository.GetById(request.Id)
                                ?? throw new InvalidOperationException("Usuario no encontrado.");

            var role = _roleRepository.GetById(request.RoleId)
                       ?? throw new InvalidOperationException("Rol no encontrado.");

            var updatedData = UserMapper.ToUpdatableData(request);
            updatedData.Role = role;

            existingUser.Update(updatedData);

            var updated = _userRepository.Update(existingUser);
            return UserMapper.ToResponse(updated);
        }

        public UserResponse DeleteUser(DeleteUserRequest request)
        {
            var deleted = _userRepository.Delete(request.Id)
                          ?? throw new InvalidOperationException("Usuario no encontrado.");

            return UserMapper.ToResponse(deleted);
        }

        public UserResponse GetUserById(int id)
        {
            var user = _userRepository.GetById(id)
                       ?? throw new InvalidOperationException("Usuario no encontrado.");

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
