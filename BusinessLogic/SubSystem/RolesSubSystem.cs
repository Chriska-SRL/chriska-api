using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.Común.Mappers;
using System.Xml.Linq;

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

            Role existing = _roleRepository.GetByName(newRole.Name);
            if (existing != null)
                throw new ArgumentException("Ya existe un rol con ese nombre.", nameof(newRole.Name));

            var added = _roleRepository.Add(newRole);
            return RoleMapper.ToResponse(added);
        }


        public RoleResponse UpdateRole(UpdateRoleRequest request)
        {
            var existingRole = _roleRepository.GetById(request.Id)
                                 ?? throw new ArgumentException("No se encontro el rol seleccionado.", nameof(request.Id));


            Role existing = _roleRepository.GetByName(request.Name);
            if (existingRole.Name != request.Name && existing != null)
                throw new ArgumentException("Ya existe un rol con ese nombre.", nameof(request.Name));

            var updatedData = RoleMapper.ToUpdatableData(request);
            existingRole.Update(updatedData);

            var updated = _roleRepository.Update(existingRole);
            return RoleMapper.ToResponse(updated);
        }

        public RoleResponse DeleteRole(int id)
        {
            var deleted = _roleRepository.Delete(id)
                          ?? throw new ArgumentException("No se encontro el rol seleccionado.", nameof(id));

            return RoleMapper.ToResponse(deleted);
        }

        public RoleResponse GetRoleById(int id)
        {
            var role = _roleRepository.GetById(id)
                       ?? throw new ArgumentException("No se encontro el rol seleccionado.", nameof(id));

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
