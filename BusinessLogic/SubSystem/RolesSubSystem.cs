using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;

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
            throw new NotImplementedException("");
           // var newRole = new Role(role.Name);
           // newRole.Validate();
            //_roleRepository.Add(newRole);

        }

        public void UpdateRole(UpdateRoleRequest role)
        {
            var updateRole = _roleRepository.GetById(role.Id);
            if (updateRole == null) throw new Exception("El rol no existe");
            updateRole.Update(role.RoleName);
            updateRole.Validate();
            _roleRepository.Update(updateRole);
        }

        public void DeleteRole(DeleteRoleRequest role)
        {
            var deleteRole = _roleRepository.GetById(role.Id);
            if (deleteRole == null) throw new Exception("El rol no existe");
            _roleRepository.Delete(deleteRole.Id);
        }

        public RoleResponse GetRoleById(int id)
        {
            var role = _roleRepository.GetById(id);
            if (role == null) throw new Exception("El rol no existe");
            var roleResponse = new RoleResponse
            {
                RoleName = role.Name
            };
            return roleResponse;
        }

        public List<RoleResponse> GetAllRoles()
        {
            var roles = _roleRepository.GetAll();
            var listRolesResponse = new List<RoleResponse>();
            foreach (var rol in roles)
            {
                var rolesResponse = new RoleResponse();
                rolesResponse.RoleName = rol.Name;
                listRolesResponse.Add(rolesResponse);
            }
            return listRolesResponse;
        }
    }
}
