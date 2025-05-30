using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.DTOs.DTOsUser;

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

        public void AddUser(AddUserRequest user)
        {
            throw new NotImplementedException("");
            /*
            var newUser = new User(user.Name, user.UserName, user.Password, user.IsEnabled, _roleRepository.GetById(user.RoleId));
            newUser.Validate();
            _userRepository.Add(newUser);
            */
        }

        public void UpdateUser(UpdateUserRequest user)
        {
            var existingUser = _userRepository.GetById(user.Id);
            if (existingUser == null) throw new Exception("No se encontro el usuario");
            existingUser.Update(user.Name, user.UserName, user.Password, user.IsEnabled, _roleRepository.GetById(user.RoleId));
            existingUser.Validate();
            _userRepository.Update(existingUser);
        }

        public void DeleteUser(User user)
        {
            var existingUser = _userRepository.GetById(user.Id);
            if (existingUser == null) throw new Exception("No se encontro el usuario");
            _userRepository.Delete(user.Id);
        }

        public UserResponse GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) throw new Exception("No se encontro el usuario");
            var userResponse = new UserResponse
            {
                Name = user.Name,
                UserName = user.Username,
                IsEnabled = user.isEnabled,
                Role = new RoleResponse
                {
                    RoleName = user.Role.Name
                }
            };
            return userResponse;
        }

        public List<UserResponse> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            if (users == null || !users.Any()) throw new Exception("No hay usuarios registrados");
            var userResponses = new List<UserResponse>();
            foreach (var user in users)
            {
                userResponses.Add(new UserResponse
                {
                    Name = user.Name,
                    UserName = user.Username,
                    IsEnabled = user.isEnabled,
                    Role = new RoleResponse
                    {
                        RoleName = user.Role.Name
                    }
                });
            }
            return userResponses;
        }
    }
}
