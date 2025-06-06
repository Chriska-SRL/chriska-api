using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Común.Mappers;

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

        public void AddUser(AddUserRequest request)
        {
            var role = _roleRepository.GetById(request.RoleId);
            if (role == null)
                throw new Exception("Rol no encontrado");

            var newUser = UserMapper.ToDomain(request);
            newUser.Validate();
            _userRepository.Add(newUser);
        }

        public void UpdateUser(UpdateUserRequest request)
        {
            var existingUser = _userRepository.GetById(request.Id);
            if (existingUser == null)
                throw new Exception("No se encontró el usuario");
           
            var role = _roleRepository.GetById(request.RoleId);
            if (role == null)
                throw new Exception("Rol no encontrado");

            var updatedData = UserMapper.ToUpdatableData(request);
            updatedData.Role = role;
            existingUser.Update(updatedData);
            _userRepository.Update(existingUser);
        }

        public void DeleteUser(DeleteUserRequest request)
        {
            if (_userRepository.Delete(request.Id) == null) throw new Exception("No se encontró el usuario");
        }

        public UserResponse GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            return UserMapper.ToResponse(user);
        }

        public List<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return users.Select(UserMapper.ToResponse).ToList();
        }
    }
}
