﻿using BusinessLogic.Dominio;
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
                needsPasswordChange: true,
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
                isEnabled = updateUserRequest.IsEnabled,
                Role = new Role(updateUserRequest.RoleId),
            };
        }

        public static UserResponse ToResponse(User user)
        {
            return new UserResponse {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                needsPasswordChange = user.needsPasswordChange,
                IsEnabled = user.isEnabled,
                Role = RoleMapper.ToResponse(user.Role),
            };
        }

    }
}
