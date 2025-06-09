using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsRole;

namespace BusinessLogic.Común.Mappers
{
    public static class RoleMapper
    {
        public static Role ToDomain(AddRoleRequest dto)
        {
            return new Role(0, dto.Name, dto.Description, dto.Permissions.Select(p => (Permission)p).ToList());
        }


        public static RoleResponse ToResponse(Role role)
        {
            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Permissions = role.Permissions.Select(p => (int)p).ToList()
            };
        }

        public static Role.UpdatableData ToUpdatableData(UpdateRoleRequest dto)
        {
            return new Role.UpdatableData
    {
                Name = dto.Name,
                Description = dto.Description,
                Permissions = dto.Permissions.Select(p => (Permission)p).ToList()
            };
        }
    }
}
