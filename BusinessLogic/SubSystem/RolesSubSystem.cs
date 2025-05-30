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

        public void AddRole(AddRoleRequest role)
        {
            Role newRole = RoleMapper.ToDomainModel(role);
            newRole.Validate();
            _roleRepository.Add(newRole);

        }

        public void UpdateRole(UpdateRoleRequest role)
        {
            Role updateRole = RoleMapper.toDomainModel(role);
            updateRole.Validate();
            if (_roleRepository.Update(updateRole) == null) throw new Exception("El rol no existe");
        }

        public void DeleteRole(DeleteRoleRequest role)
        {
            if (_roleRepository.Delete(role.Id) == null) throw new Exception("El rol no existe");
        }

        public RoleResponse GetRoleById(int id)
        {
            Role? role = _roleRepository.GetById(id);
            if(role == null) throw new Exception("El rol no existe");
            return RoleMapper.toDTO(role);
        }

        public List<RoleResponse> GetAllRoles()
        {
            return  _roleRepository.GetAll().Select(r => RoleMapper.toDTO(r)).ToList();
        }
    }
}
