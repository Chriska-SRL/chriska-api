using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.Común.Mappers
{
    public static class UserMapper
    {
        public static User ToDomain(AddUserRequest addUserRequest)
        {
            return new User(
                id: 0,
                name: addUserRequest.Name,
                username: addUserRequest.UserName,
                password: "",
                isEnabled: addUserRequest.IsEnabled,
                role: new Role(
                    id: addUserRequest.RoleId,
                    name:"",
                    description: "",
                    permissions: new List<Permission>()
                    ),
                requests: new List<Request>()
            );
        }
        public static User.UpdatableData ToUpdatableData(UpdateUserRequest updateUserRequest)
        {
            return new User.UpdatableData
            {
                Name = updateUserRequest.Name,
                Username = updateUserRequest.UserName,
                isEnabled = updateUserRequest.IsEnabled,
                Role = null // Role will be set later in the update process
            };
        }

        public static UserResponse ToResponse(User user)
        {
            return new UserResponse {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                IsEnabled = user.isEnabled,
                Role = new RoleResponse
                {
                    Id = user.Role.Id,
                    Name = user.Role.Name,
                    Permissions = user.Role.Permissions.Select(p => (int)p).ToList()
                }

            };
        }

    }
}
