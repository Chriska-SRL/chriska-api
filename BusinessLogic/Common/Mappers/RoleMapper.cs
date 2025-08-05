using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsRole;

namespace BusinessLogic.Common.Mappers
{
    public static class RoleMapper
    {
        public static Role ToDomain(AddRoleRequest request)
        {
            var role = new Role(
                name: request.Name,
                description: request.Description,
                permissions: request.Permissions.Select(p => (Permission)p).ToList()
            );

            role.AuditInfo.SetCreated(request.getUserId(), request.Location);
            return role;
        }

        public static Role.UpdatableData ToUpdatableData(UpdateRoleRequest request)
        {
            return new Role.UpdatableData
            {
                Name = request.Name,
                Description = request.Description,
                Permissions = request.Permissions.Select(p => (Permission)p).ToList(),
                UserId = request.getUserId(),
                Location = request.Location
            };
        }

        public static RoleResponse ToResponse(Role role)
        {
            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Permissions = role.Permissions.Select(p => (int)p).ToList(),
                AuditInfo = AuditMapper.ToResponse(role.AuditInfo)
            };
        }
    }
}
