using BusinessLogic.Común.Mappers;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Repository;

namespace BusinessLogic.SubSystem
{
    public class AuthSubSystem
    {
        private readonly IUserRepository _userRepository;

        public AuthSubSystem(IUserRepository userRepository)
        {
            _userRepository = userRepository;;
        }

        public UserResponse? Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null || user.Password != password || !user.isEnabled)
                return null;

            return UserMapper.ToResponse(user);
        }
    }
}
