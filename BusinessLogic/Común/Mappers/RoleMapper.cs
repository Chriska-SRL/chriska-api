using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsRole;

namespace BusinessLogic.Común.Mappers
{
    public static class RoleMapper
    {

        public static Role ToDomainModel(AddRoleRequest request)
        {
            return new Role(
                id: 0,
                name: request.Name,
                permissions: request.PermissionsIds.Select(id => (Permission)id).ToList()
           );
        }

        public static Role toDomainModel(UpdateRoleRequest request) 
        {
            return new Role(
               id: request.Id,
               name: request.Name,
               permissions: request.PermissionsIds.Select(id => (Permission)id).ToList()
          );
        }

        public static RoleResponse toDTO(Role role)
        {
            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                PermissionsIds = role.Permissions.Select(p => (int)p).ToList()
            };
        }
    }
}
