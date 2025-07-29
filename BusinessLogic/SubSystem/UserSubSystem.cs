using BusinessLogic.Común;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsUser;
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

        public async Task<UserResponse> AddUserAsync(AddUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ResetPasswordAsync(int userId, string? newPassword = null)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(DeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserResponse>> GetAllUsersAsync(QueryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
