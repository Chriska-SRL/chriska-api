using BusinessLogic.Common.Mappers;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.Repository;
using System.Threading.Tasks;

namespace BusinessLogic.SubSystem
{
    public class AuthSubSystem
    {
        private readonly IUserRepository _userRepository;

        public AuthSubSystem(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password) || !user.IsEnabled)
                return null;

            return UserMapper.ToResponse(user);
        }
    }
}
