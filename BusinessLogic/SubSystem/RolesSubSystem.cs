using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.Común.Mappers;

namespace BusinessLogic.SubSystem
{
    public class RolesSubSystem
    {
        private readonly IRoleRepository _roleRepository;

        public RolesSubSystem(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public RoleResponse AddRole(AddRoleRequest request)
        {
            var newRole = RoleMapper.ToDomain(request);
            newRole.Validate();

            var added = _roleRepository.Add(newRole);
            return RoleMapper.ToResponse(added);
        }

        public RoleResponse UpdateRole(UpdateRoleRequest request)
        {
            var existingRole = _roleRepository.GetById(request.Id)
                                 ?? throw new InvalidOperationException("Rol no encontrado.");

            var updatedData = RoleMapper.ToUpdatableData(request);
            existingRole.Update(updatedData);

            var updated = _roleRepository.Update(existingRole);
            return RoleMapper.ToResponse(updated);
        }

        public RoleResponse DeleteRole(DeleteRoleRequest request)
        {
            var deleted = _roleRepository.Delete(request.Id)
                          ?? throw new InvalidOperationException("Rol no encontrado.");

            return RoleMapper.ToResponse(deleted);
        }

        public RoleResponse GetRoleById(int id)
        {
            var role = _roleRepository.GetById(id)
                       ?? throw new InvalidOperationException("Rol no encontrado.");

            return RoleMapper.ToResponse(role);
        }

        public List<RoleResponse> GetAllRoles()
        {
            return _roleRepository.GetAll()
                                  .Select(RoleMapper.ToResponse)
                                  .ToList();
        }
    }
}
