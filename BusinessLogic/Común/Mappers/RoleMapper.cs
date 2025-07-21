using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsRole;

namespace BusinessLogic.Común.Mappers
{
    public static class RoleMapper
    {
        public static Role ToDomain(AddRoleRequest dto)
        {
            return new Role(
                id: 0,
                name: dto.Name,
                description: dto.Description,
                permissions: dto.Permissions.Select(p => (Permission)p).ToList(),
                auditInfo: AuditMapper.ToDomain(dto.AuditInfo)
            );
        }

        public static Role.UpdatableData ToUpdatableData(UpdateRoleRequest dto)
        {
            return new Role.UpdatableData
            {
                Name = dto.Name,
                Description = dto.Description,
                Permissions = dto.Permissions.Select(p => (Permission)p).ToList(),
                AuditInfo = AuditMapper.ToDomain(dto.AuditInfo)
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
