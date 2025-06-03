using BusinessLogic.Repository;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.Común.Mappers;
using BusinessLogic.Dominio;

namespace BusinessLogic.SubSystem
{
    public class RolesSubSystem
    {
        private readonly IRoleRepository _roleRepository;

        public RolesSubSystem(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void AddRole(AddRoleRequest request)
        {
            Role role = RoleMapper.ToDomain(request);
            role.Validate();
            _roleRepository.Add(role);
        }

        public void UpdateRole(UpdateRoleRequest request)
        {
            Role? role = _roleRepository.GetById(request.Id);
            if (role == null)
                throw new Exception("El rol no existe");

            role.Update(RoleMapper.ToUpdatableData(request));
            _roleRepository.Update(role);
        }

        public void DeleteRole(DeleteRoleRequest request)
        {
            Role? role = _roleRepository.GetById(request.Id);
            if (role == null)
                throw new Exception("El rol no existe");

            _roleRepository.Delete(role.Id);
        }

        public RoleResponse GetRoleById(int id)
        {
            Role? role = _roleRepository.GetById(id);
            if (role == null)
                throw new Exception("Rol no encontrado");

            return RoleMapper.ToResponse(role);
        }

        public List<RoleResponse> GetAllRoles()
        {
            List<Role> roles = _roleRepository.GetAll();
            List<RoleResponse> response = roles.Select(RoleMapper.ToResponse).ToList();
            return response;
        }
    }
}
