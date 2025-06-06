using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsAuth;
using BusinessLogic.DTOs.DTOsCategory;
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
           throw new NotImplementedException("Login method is not implemented yet.");
        }
    }
}
