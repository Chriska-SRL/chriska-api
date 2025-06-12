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
                username: addUserRequest.Username,
                password: "Temporal.12345",
                isEnabled: addUserRequest.IsEnabled,
                role: new Role(addUserRequest.RoleId),
                requests: new List<Request>()
            );
        }
        public static User.UpdatableData ToUpdatableData(UpdateUserRequest updateUserRequest)
        {
            return new User.UpdatableData
            {
                Name = updateUserRequest.Name,
                Username = updateUserRequest.Username,
                IsEnabled = updateUserRequest.IsEnabled,
                Role = new Role(updateUserRequest.RoleId),
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
                    Description = user.Role.Description,
                    Permissions = user.Role.Permissions.Select(p => (int)p).ToList()
                }
            };
        }

    }
}
