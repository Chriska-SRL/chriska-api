using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.Common.Mappers
{
    public static class UserMapper
    {
        public static User ToDomain(AddUserRequest request)
        {
            var user = new User(
                name: request.Name,
                username: request.Username,
                password: "Temporal.12345",
                isEnabled: request.IsEnabled,
                needsPasswordChange: true,
                role: new Role(request.RoleId)
            );

            user.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return user;
        }

        public static User.UpdatableData ToUpdatableData(UpdateUserRequest request)
        {
            return new User.UpdatableData
            {
                Name = request.Name,
                Username = request.Username,
                IsEnabled = request.IsEnabled,
                Role = new Role(request.RoleId),

                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static UserResponse ToResponse(User? user)
        {
            if (user == null)
            {
                return null;
            }
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                needsPasswordChange = user.NeedsPasswordChange,
                IsEnabled = user.IsEnabled,
                Role = RoleMapper.ToResponse(user.Role),
                AuditInfo = AuditMapper.ToResponse(user.AuditInfo)
            };
        }
    }
}
